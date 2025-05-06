using DotnetGeminiSDK;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HireHive.Infrastructure.Startup
{
    public static class GeminiConfiguration
    {
        public static void ConfigureGemini(IServiceCollection services, IConfiguration configuration)
        {
            services.AddGeminiClient(config =>
            {
                config.ApiKey = configuration["GeminiApiKey"]!;
            });
        }
    }
}
