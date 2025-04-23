using Application.Interfaces;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("HireHiveDbConnectionString");
        Guard.Against.Null(connectionString, message: "Connection string 'HireHiveDbConnectionString' not found.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<AppDbContextInitialiser>();

        services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        services.AddAuthentication();
        services.AddAuthorization();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IResumeRepository, ResumeRepository>();
        services.AddScoped<IAuthService, AuthService>();
    }
}
