using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Domain.Interfaces;
using Infrastructure.Data;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Infrastructure.Data.Repositories;
using Infrastructure.Identity;
using Infrastructure.Mappings;
//using AutoMapper.Extensions.Microsoft.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("HireHiveDbConnectionString");
        Guard.Against.Null(connectionString, message: "Connection string 'HireHiveDbConnectionString' not found.");

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));


        builder.Services.AddScoped<AppDbContextInitialiser>();

        builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        //builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IResumeRepository, ResumeRepository>();
    }
}
