using HireHive.Domain.Entities;
using System.Text;

namespace HireHive.Infrastructure.Services.AI
{
    public class PromtBuilderService
    {
        public BuildPromt(List<User> users)
        {
            var promptBuilder = new StringBuilder();
            foreach (var user in users)
            {
                promptBuilder.AppendLine($"Candidate_{user.Id}:");
                promptBuilder.AppendLine(user.ResumeText.Trim());
                promptBuilder.AppendLine("---");
            }

            promptBuilder.AppendLine();
            promptBuilder.AppendLine("Please return one JSON object per candidate, one per line, like:");
            promptBuilder.AppendLine(@"{ ""name"": ""Candidate_4fdba6f2-1234-43ab-9f80-5e25a8ea1097"", ""match"": 87, ""reason"": ""Strong backend experience"" }");

            return promptBuilder.ToString();
        }

    }
}
