using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure.Storage.Blobs;
using HireHive.Application.Interfaces;
using HireHive.DependencyInjection;
using HireHive.Domain.Interfaces;
using HireHive.Infrastructure.Data;
using HireHive.Infrastructure.Data.Repositories;
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
            services.AddScoped<IResumeService, ResumeService>();

            var blobConnectionString = configuration["AzureBlobStorageConnection"];
            var blobSasToken = configuration["BlobSaSToken"];
            var blobClient = new BlobServiceClient(blobConnectionString);

            services.AddSingleton(blobClient);
            services.AddSingleton(blobSasToken!);
            services.AddScoped<IAzureBlobService, AzureBlobService>();

            var emailSettings = new EmailSettings
            {
                ApiKey = configuration["SendGridApiKey"]!,
                FromEmail = configuration["EmailSettings:From"]!,
                Name = configuration["EmailSettings:Name"]!
            };

            services.AddSingleton(emailSettings);
            services.AddTransient<IEmailService, EmailService>();

            var apiKey = configuration["DocumentIntelligenceApiKey"];
            var endpoint = configuration["DocIntelligenceSettings:Endpoint"];
            var docIntelligenceClient = new DocumentAnalysisClient(new Uri(endpoint!), new AzureKeyCredential(apiKey!));

            services.AddSingleton(docIntelligenceClient);
            services.AddScoped<DocumentIntelligenceService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IResumeRepository, ResumeRepository>();
        }
    }
}
