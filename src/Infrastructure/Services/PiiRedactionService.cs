using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure.AI.TextAnalytics;
using HireHive.Application.Interfaces;
using System.Text;

namespace HireHive.Infrastructure.Services
{
    public class PiiRedactionService : IPiiRedactionService
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly TextAnalyticsClient _textAnalyticsClient;
        private readonly DocumentAnalysisClient _documentIntelligenceClient;
        public PiiRedactionService(IAzureBlobService azureBlobService, TextAnalyticsClient textAnalyticsClient, DocumentAnalysisClient documentIntelligenceClient)
        {
            _azureBlobService = azureBlobService;
            _textAnalyticsClient = textAnalyticsClient;
            _documentIntelligenceClient = documentIntelligenceClient;

        }

        public async Task<string> ExtractBlobText(string blobName)
        {
            await documentStream = await _azureBlobService.GetBlobUri(blobName);

            AnalyzeDocumentOperation operation = await _documentIntelligenceClient.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-document", documentStream);

            AnalyzeResult result = operation.Value;

            var documentText = string.Join(" ", result.Pages
                .SelectMany(p => p.Lines)
                .Select(l => l.Content));

            return documentText;
        }

        public async Task<string> RedactPii(string blobName)
        {
            var blobUri = _azureBlobService.GetBlobUri(blobName);

            var documentText = await ExtractBlobText(blobUri);

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
                if (entity.Offset is int offset && entity.Length is int length)
                {
                    sb.Append(documentText.Substring(position, offset - position));
                    sb.Append(new string('*', length));
                    position = offset + length;
                }
            }

            sb.Append(documentText.Substring(position));
            var redactedDocumentText = sb.ToString();

            return redactedDocumentText;
        }
    }
}
