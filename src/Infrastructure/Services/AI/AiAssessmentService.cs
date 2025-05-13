using Azure;
using Azure.AI.Inference;
using HireHive.Domain.Entities;
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

        public async Task Chat(Guid userId)
        {
            var resume = await _resumeRepository.GetByUserIdAsync(userId)
                ?? throw new ResumeNotFoundException();

            var resume2 = await _resumeRepository.GetByUserIdAsync(new Guid("0196a012-7461-74d7-a2af-7ec497432399"))
                ?? throw new ResumeNotFoundException();
            _logger.LogInformation("AI assessment: ");

            var resumes = new List<Resume> { resume, resume2 };

            string prompt = @"
                You are an HR specialist responsible for evaluating how well candidate resumes match a given job description.

                You will receive:
                - A single job description
                - Multiple resumes in plain text format

                Your task is to assess each resume providing a numeric percentage (0 to 100) indicating how closely the resume matches the job description. 
                You refer to the candidate with their user id. 
                Do not include any explanation or text outside the JSON response.
                ";

            string description = "Job Title: Software Engineer (Backend)\r\nLocation: Hybrid – New York, NY\r\nType: Full-time\r\n\r\nAbout the Role:\r\nWe’re seeking a backend-focused Software Engineer to join our team. You'll build scalable APIs and microservices using C# and .NET, helping power our data platform.\r\n\r\nKey Responsibilities:\r\nDevelop and maintain backend services in .NET 6+\r\n\r\nCollaborate with cross-functional teams\r\n\r\nWrite clean, tested, and maintainable code\r\n\r\nTroubleshoot production issues\r\n\r\nRequirements:\r\n3+ years of experience with C#/.NET\r\n\r\nProficient in SQL/NoSQL databases\r\n\r\nFamiliarity with Docker, CI/CD, and cloud platforms (AWS/Azure)\r\n\r\nBonus Points:\r\nKnowledge of Kafka, GraphQL, or DDD\r\n\r\nPerks:\r\nCompetitive salary + benefits\r\n\r\nFlexible PTO & remote work\r\n\r\nHealth, dental, 401(k), and more";

            var chatHistory = new List<ChatRequestMessage>(){
                new ChatRequestSystemMessage(prompt),
                new ChatRequestUserMessage(description),
            };

            foreach (var r in resumes)
            {
                chatHistory.Add(new ChatRequestUserMessage(r.UserId + r.Text));
            }

            var requestOptions = new ChatCompletionsOptions(chatHistory)
            {
                Model = "openai/gpt-4.1",
                ResponseFormat = new ChatCompletionsResponseFormatJsonObject(),
            };
            //ChatCompletionsToolDefinition evaluateCandidateTool = new ChatCompletionsToolDefinition(EvaluateCandidateTool.GetToolDefinition());

            Response<ChatCompletions> response = _client.Complete(requestOptions);
            _logger.LogInformation("", response);
            //return assessment;
        }
    }
}
