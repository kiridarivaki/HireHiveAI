using Azure;
using Azure.AI.Inference;
using HireHive.Application.DTOs.Admin;
using HireHive.Application.Interfaces;
using HireHive.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharpToken;
using System.Text.Json;

namespace HireHive.Infrastructure.Services
{
    public class AssessmentService : IAssessmentService
    {
        const int MAX_TOKENS = 8000;
        private readonly ChatCompletionsClient _client;
        private readonly ILogger<AssessmentService> _logger;
        public AssessmentService(ChatCompletionsClient client, ILogger<AssessmentService> logger)
        {
            _client = client;
            _logger = logger;
        }
        public int CountTokens(string? text)
        {
            var encoding = GptEncoding.GetEncodingForModel("gpt-4");
            int count = 0;
            if (text != null)
                count = encoding.CountTokens(text);

            return count;
        }
        public bool isTokenLimitReached(int tokenCount)
        {
            return tokenCount > MAX_TOKENS;
        }

        private string BuildAssessmentPrompt(AssessmentParamsDto assessmentDto)
        {
            var criteriaWithWeights = new List<string>();
            string criteria;

            if (assessmentDto.Criteria != null && assessmentDto.CriteriaWeights != null &&
                assessmentDto.Criteria.Count == assessmentDto.CriteriaWeights.Count)
            {
                foreach (var item in assessmentDto.Criteria.Zip(assessmentDto.CriteriaWeights, (c, w) => new { Criteria = c, Weight = w }))
                {
                    criteriaWithWeights.Add($"- {item.Criteria}: {item.Weight}");
                }

                criteria = string.Join("\n", criteriaWithWeights);
            }
            else
            {
                criteria = "No criteria was defined.";
            }

            string prompt = @$"
                You are an HR resume assessment specialist. Your task is to evaluate provided resumes against a job description.
                Assess each resume's match to the job description, considering:
                {criteria}

                For each candidate you assess, generate one JSON object.
                Each object MUST have the following properties:
                - 'UserId': (GUID)
                - 'MatchPercentage': (0-100 numeric)
                - 'Explanation': A concise evaluation (max 3 sentences).

                The entire response must be a JSON array of these assessment objects.
                Even if there is only one candidate, the single object must be wrapped in an array.
                The response MUST start with `[` and end with `]`, and contain ONLY the JSON array (no other text or formatting).
                ";

            return prompt;
        }

        public async Task<List<AssessmentResultDto>> AssessUsers(List<UserResumeDto> usersToAssess, AssessmentParamsDto assessmentDto)
        {
            var prompt = BuildAssessmentPrompt(assessmentDto);

            var contentItems = new List<ChatMessageContentItem>();


            foreach (var resume in usersToAssess)
            {
                var resumeText = $"UserId: {resume.UserId}\nResume: {resume.ResumeText}";
                contentItems.Add(new ChatMessageTextContentItem(resumeText));
            }

            var userMessage = new ChatRequestUserMessage(contentItems);

            var chatHistory = new List<ChatRequestMessage> {
                new ChatRequestSystemMessage(prompt),
                new ChatRequestUserMessage(assessmentDto.JobDescription),
                userMessage
            };

            var requestOptions = new ChatCompletionsOptions(chatHistory)
            {
                Model = "openai/gpt-4.1"
            };

            try
            {
                Response<ChatCompletions> response = await _client.CompleteAsync(requestOptions);
                var jsonResponse = response.Value.Content;

                var assessmentResults =
                    JsonSerializer.Deserialize<List<AssessmentResultDto>>(jsonResponse,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return assessmentResults!;
            }
            catch (JsonException e)
            {
                _logger.LogWarning("JSON Deserialization Error. With exception: {message}", e.Message);
                throw;
            }
            catch (RequestFailedException e)
            {
                _logger.LogWarning("Azure AI Service request failed. With exception: {message}", e.Message);
                throw;
            }
            catch (BaseException e)
            {
                _logger.LogWarning("AI Assessment failed. With exception: {message}", e.Message);
                throw;
            }
        }
    }
}
