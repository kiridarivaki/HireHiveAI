using Azure;
using Azure.AI.Inference;
using HireHive.Application.DTOs.Admin;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace HireHive.Infrastructure.Services
{
    public class AiAssessmentService
    {
        private readonly ChatCompletionsClient _client;
        private readonly ILogger<AiAssessmentService> _logger;
        public AiAssessmentService(ChatCompletionsClient client, ILogger<AiAssessmentService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public List<AssessmentResultDto> AssessUsers(List<UserResumeDto> usersToAssess, AssessmentParamsDto assessmentDto)
        {
            var experienceWeight = assessmentDto.CriteriaWeights[0];
            var educationWeight = assessmentDto.CriteriaWeights[1];
            var skillsWeight = assessmentDto.CriteriaWeights[2];

            string prompt = @$"
                You are an HR specialist responsible for evaluating how well candidate resumes match a given job description.

                You will receive:
                - A single job description
                - A list of resumes in plain text format

                You consider most important the assessment criteria with the most weight. Criteria weights:
                - Education: {experienceWeight}
                - Education: {educationWeight}
                - Skills: {skillsWeight}

                Your task is to assess each resume providing a numeric percentage (0 to 100) indicating how closely the resume matches the job description. 
                You refer to the candidate with their user id. 
                Do not include any explanation or text outside the JSON response.
                ";

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
                Model = "openai/gpt-4.1",
                ResponseFormat = new ChatCompletionsResponseFormatJsonObject(),
            };

            Response<ChatCompletions> response = _client.Complete(requestOptions);

            var jsonResponse = response.Value.Content;
            var dictResponse = JsonSerializer.Deserialize<Dictionary<Guid, int>>(jsonResponse);

            var assessmentResultDto = dictResponse.Select(kvp => new AssessmentResultDto
            {
                UserId = kvp.Key,
                MatchPercentage = kvp.Value
            }).ToList();
            _logger.LogInformation(jsonResponse);
            return assessmentResultDto;
        }
    }
}
