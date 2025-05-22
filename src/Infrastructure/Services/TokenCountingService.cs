using SharpToken;

namespace HireHive.Infrastructure.Services
{
    public class TokenCountingService
    {
        const int MAX_TOKENS = 34815;
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
    }
}
