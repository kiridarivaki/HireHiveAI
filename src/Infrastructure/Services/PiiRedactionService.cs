using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure.AI.TextAnalytics;
using HireHive.Application.Interfaces;
using System.Text;

namespace HireHive.Infrastructure.Services
{
    public class PiiRedactionService : IPiiRedactionService
    {
        private readonly TextAnalyticsClient _textAnalyticsClient;
        private readonly DocumentAnalysisClient _documentIntelligenceClient;
        public PiiRedactionService(TextAnalyticsClient textAnalyticsClient, DocumentAnalysisClient documentIntelligenceClient)
        {
            _textAnalyticsClient = textAnalyticsClient;
            _documentIntelligenceClient = documentIntelligenceClient;

        }

        public async Task<string> ExtractText(MemoryStream documentStream)
        {
            AnalyzeDocumentOperation operation = await _documentIntelligenceClient.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-document", documentStream);

            AnalyzeResult result = operation.Value;

            var documentText = string.Join(" ", result.Pages
                .SelectMany(p => p.Lines)
                .Select(l => l.Content));

            return documentText;
        }

        public async Task<string> RedactPii(string documentText)
        {
            Response<PiiEntityCollection> piiResult = await _textAnalyticsClient.RecognizePiiEntitiesAsync(documentText);

            var categoriesToRedact = new List<PiiEntityCategory>()
            {
                PiiEntityCategory.Email,
                PiiEntityCategory.Address,
                PiiEntityCategory.PhoneNumber,
                PiiEntityCategory.Age
            };

            var entitiesToRedact = piiResult.Value
                .Where(e => categoriesToRedact.Contains(e.Category) && e.ConfidenceScore >= 0.6)
                .OrderBy(e => e.Offset);

            var sb = new StringBuilder();
            int position = 0;

            foreach (var entity in entitiesToRedact)
            {
                int offset = entity.Offset;
                int length = entity.Length;

                sb.Append(documentText.Substring(position, offset - position));
                sb.Append(new string('*', length));
                position = offset + length;
            }

            sb.Append(documentText.Substring(position));
            var redactedDocumentText = sb.ToString();

            return redactedDocumentText;
        }
    }
}
