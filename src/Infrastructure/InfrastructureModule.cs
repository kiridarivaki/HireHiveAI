using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure.AI.Inference;
using Azure.AI.TextAnalytics;
using Azure.Storage.Blobs;
using HireHive.Application.Interfaces;
using HireHive.DependencyInjection;
using HireHive.Domain.Interfaces;
using HireHive.Infrastructure.Data;
using HireHive.Infrastructure.Data.Repositories;
using HireHive.Infrastructure.Jobs;
using HireHive.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HireHive.Infrastructure
{
    class InfrastructureModule : IModule
    {
        public void ConfigureDependencyInjection(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["HireHivePostgresConnectionString"];

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<Application.Interfaces.IResumeService, ResumeService>();
            services.AddScoped<IAdminService, AdminService>();

            var blobConnectionString = configuration["AzureBlobStorageConnection"];
            var blobSasToken = configuration["BlobSaSToken"];
            var blobClient = new BlobServiceClient(blobConnectionString);

            services.AddSingleton(blobClient);
            services.AddSingleton(blobSasToken!);
            services.AddScoped<IBlobService, BlobService>();

            var emailSettings = new EmailSettings
            {
                ApiKey = configuration["SendGridApiKey"]!,
                FromEmail = configuration["EmailSettings:From"]!,
                Name = configuration["EmailSettings:Name"]!
            };

            services.AddSingleton(emailSettings);
            services.AddTransient<IEmailService, EmailService>();

            var diApiKey = configuration["DocumentIntelligenceApiKey"];
            var diEndpoint = configuration["DocIntelligenceSettings:Endpoint"];
            var diClient = new DocumentAnalysisClient(new Uri(diEndpoint!), new AzureKeyCredential(diApiKey!));

            var piiApiKey = configuration["PIIDetectionApiKey"];
            var piiEndpoint = configuration["PIIDetectionSettings:Endpoint"];
            var piiClient = new TextAnalyticsClient(new Uri(piiEndpoint!), new AzureKeyCredential(piiApiKey!));

            services.AddSingleton(piiClient);
            services.AddSingleton(diClient);

            services.AddScoped<IPiiRedactionService, PiiRedactionService>();
            services.AddScoped<IResumeProcessingJob, ResumeProcessingJob>();

            var githubEndpoint = new Uri("https://models.github.ai/inference");
            var credential = new AzureKeyCredential(configuration["GitHubToken"]!);

            var aiClient = new ChatCompletionsClient(githubEndpoint, credential, new AzureAIInferenceClientOptions());

            services.AddSingleton(aiClient);
            services.AddScoped<IAiAssessmentService, AiAssessmentService>();
            services.AddScoped<TokenCounterService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IResumeRepository, ResumeRepository>();
        }
    }
}
