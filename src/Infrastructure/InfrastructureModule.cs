using Azure.Storage.Blobs;
using HireHive.Application.Interfaces;
using HireHive.DependencyInjection;
using HireHive.Domain.Interfaces;
using HireHive.Infrastructure.Data;
using HireHive.Infrastructure.Data.Repositories;
using HireHive.Infrastructure.FileStorage;
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
            services.AddScoped<TokenService>();

            var blobConnectionString = configuration["AzureBlobStorageConnection"];
            var blobClient = new BlobServiceClient(blobConnectionString);

            services.AddSingleton(blobClient);
            services.AddScoped<IAzureBlobService, AzureBlobService>();

            var emailApiKey = configuration["SendGridApiKey"];

            var emailSettings = new EmailSettings
            {
                FromEmail = configuration["EmailSettings:From"]!,
                Name = configuration["EmailSettings:Name"]!
            };

            services.AddSingleton<string>(configuration["SendGridApiKey"]!);
            services.AddSingleton(emailSettings);

            services.AddTransient<IEmailService, EmailService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IResumeRepository, ResumeRepository>();
        }
    }
}
