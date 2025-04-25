using Azure.Storage.Blobs;
using HireHive.Application.Interfaces;
using HireHive.DependencyInjection;
using HireHive.Domain.Entities;
using HireHive.Domain.Interfaces;
using HireHive.Infrastructure.Data;
using HireHive.Infrastructure.Data.Repositories;
using HireHive.Infrastructure.FileStorage;
using HireHive.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
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

            services.AddScoped<Initialiser>();

            services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IResumeService, ResumeService>();
            services.AddScoped<IAzureBlobService, AzureBlobService>();

            services.AddSingleton<BlobServiceClient>(provider =>
            {
                var blobConnectionString = configuration["AzureBlobStorageConnection"];
                return new BlobServiceClient(blobConnectionString);
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IResumeRepository, ResumeRepository>();
        }
    }
}
