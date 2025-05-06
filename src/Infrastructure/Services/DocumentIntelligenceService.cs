using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;

namespace HireHive.Infrastructure.Services
{
    public class DocumentIntelligenceService
    {
        private readonly DocumentAnalysisClient _documentClient;
        public DocumentIntelligenceService(DocumentAnalysisClient documentClient)
        {
            _documentClient = documentClient;
        }

        public async Task ExtractKeywordsAsync(string blobUri)
        {
            using var httpClient = new HttpClient();
            using var stream = await httpClient.GetStreamAsync(blobUri);
            using var memoryStream = new MemoryStream();

            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            AnalyzeDocumentOperation operation = await _documentClient.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-document", memoryStream);
            var result = operation.Value;
            foreach (var kvp in result.KeyValuePairs)
            {
                string key = kvp.Key.Content;
                string value = kvp.Value?.Content ?? "<no value>";
                Console.WriteLine($"Key: {key} | Value: {value}");
            }


        }
    }
}
