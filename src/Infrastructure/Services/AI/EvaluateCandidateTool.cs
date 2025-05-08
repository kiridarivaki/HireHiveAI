using Azure.AI.Inference;
using System.Text.Json;

namespace HireHive.Infrastructure.Services.AI
{
    public class EvaluateCandidateTool
    {
        public static FunctionDefinition GetToolDefinition()
        {
            FunctionDefinition candidateEvaluationFunction = new FunctionDefinition("evaluateCandidateTool")
            {
                Description = "Evaluates a candidate resume depending on how much they match a job description.",
                Parameters = BinaryData.FromObjectAsJson(new
                {
                    Type = "object",
                    Properties = new
                    {
                        candidate = new
                        {
                            Type = "string",
                            Description = "The word candidate_ followed by the canididate number."
                        },
                        matchPercentage = new
                        {
                            Type = "string",
                            Description = "The percentage the resume matches the job description."
                        }
                    }
                },
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            };

            return candidateEvaluationFunction;
        }
        public string evaluateCandidateTool(string candidate, string matchPercentage)
        {
            var result = new Dictionary<string, string>
            {
                { candidate, matchPercentage }
            };

            return JsonSerializer.Serialize(result);
        }

    }
}
