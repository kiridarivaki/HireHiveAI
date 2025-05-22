using Azure.AI.Inference;
using System.Text.Json;

namespace HireHive.Infrastructure.Services.AI
{
    public class MatchJobTool
    {
        public static FunctionDefinition GetToolDefinition()
        {
            FunctionDefinition matchJobFunction = new FunctionDefinition("matchJobTool")
            {
                Description = "Evaluates the resumes of candidates depending on how much they match a job description.",
                Parameters = BinaryData.FromObjectAsJson(new
                {
                    Type = "object",
                    Properties = new
                    {
                        resumeTexts = new
                        {
                            Type = "array",
                            Description = "A list of candidate resume texts."
                        },
                        jobDescription = new
                        {
                            Type = "string",
                            Description = "The job description."
                        },
                        cursor = new
                        {
                            Type = "int",
                            Description = "The number of the last assessed candidate."
                        },
                        criteriaWeights = new
                        {
                            Type = "array",
                            Description = "The criteria weights for the assessment."
                        }
                    }
                },
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            };

            return matchJobFunction;
        }
        public void matchJobTool(string jobDescription, List<string> resumeTexts, List<int> criteriaWeights)
        {

        }
    }
}
