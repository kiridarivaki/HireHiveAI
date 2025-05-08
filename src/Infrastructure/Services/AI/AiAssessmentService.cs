using Azure;
using Azure.AI.Inference;
using HireHive.Domain.Exceptions.Resume;
using HireHive.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HireHive.Infrastructure.Services.AI
{
    public class AiAssessmentService
    {
        private readonly ChatCompletionsClient _client;
        private readonly IResumeRepository _resumeRepository;
        private readonly ILogger<AiAssessmentService> _logger;
        public AiAssessmentService(ChatCompletionsClient client, IResumeRepository resumeRepository, ILogger<AiAssessmentService> logger)
        {
            _client = client;
            _resumeRepository = resumeRepository;
            _logger = logger;
        }

        public async Task<string> Chat(Guid userId)
        {
            var resume = await _resumeRepository.GetByUserIdAsync(userId)
                ?? throw new ResumeNotFoundException();

            var resume2 = await _resumeRepository.GetByUserIdAsync(new Guid("0196a012-7461-74d7-a2af-7ec497432399"))
                ?? throw new ResumeNotFoundException();
            _logger.LogInformation("AI assessment: ");

            string prompt = @"
                You are an HR specialist responsible for evaluating how well candidate resumes match a given job description.

                You will receive:
                - A single job description
                - Multiple resumes in plain text format

                Your task is to assess each resume and return a JSON object. Each key should be a candidate number (e.g., ""candidate_1"", ""candidate_2"", etc.), and each value should be a numeric percentage (0 to 100) indicating how closely the resume matches the job description.

                Respond only with a valid JSON object in the following format:

                {
                  ""candidate_1"": 85,
                  ""candidate_2"": 67,
                  ""candidate_3"": 92
                }

                Do not include any explanation or text outside the JSON response.
                ";

            string description = "Job Title: Software Engineer (Backend)\r\nLocation: Hybrid – New York, NY\r\nType: Full-time\r\n\r\nAbout the Role:\r\nWe’re seeking a backend-focused Software Engineer to join our team. You'll build scalable APIs and microservices using C# and .NET, helping power our data platform.\r\n\r\nKey Responsibilities:\r\nDevelop and maintain backend services in .NET 6+\r\n\r\nCollaborate with cross-functional teams\r\n\r\nWrite clean, tested, and maintainable code\r\n\r\nTroubleshoot production issues\r\n\r\nRequirements:\r\n3+ years of experience with C#/.NET\r\n\r\nProficient in SQL/NoSQL databases\r\n\r\nFamiliarity with Docker, CI/CD, and cloud platforms (AWS/Azure)\r\n\r\nBonus Points:\r\nKnowledge of Kafka, GraphQL, or DDD\r\n\r\nPerks:\r\nCompetitive salary + benefits\r\n\r\nFlexible PTO & remote work\r\n\r\nHealth, dental, 401(k), and more";

            var chatHistory = new List<ChatRequestMessage>(){
                new ChatRequestSystemMessage(prompt),
                new ChatRequestUserMessage(description),
                new ChatRequestUserMessage(resume2.Text),
                new ChatRequestUserMessage(resume.Text)
            };

            var requestOptions = new ChatCompletionsOptions(chatHistory) { Model = "gpt-3.5-turbo" };
            ChatCompletionsToolDefinition evaluateCandidateTool = new ChatCompletionsToolDefinition(EvaluateCandidateTool.GetToolDefinition());

            requestOptions.Tools.Add(evaluateCandidateTool);
            requestOptions.ToolChoice = ChatCompletionsToolChoice.Auto;

            Response<ChatCompletions> response = _client.Complete(requestOptions);
            var assessment = response.Value.Content;

            _logger.LogInformation("AI assessment: {assessment}", assessment);

            return assessment;
        }
    }
}
