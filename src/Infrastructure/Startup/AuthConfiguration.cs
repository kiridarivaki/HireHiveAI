using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HireHive.Infrastructure.Startup;

public static class AuthConfiguration
{
    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
            o.DefaultChallengeScheme = IdentityConstants.BearerScheme;
            o.DefaultSignInScheme = IdentityConstants.BearerScheme;
        })
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddOptions<BearerTokenOptions>(IdentityConstants.BearerScheme).Configure(x =>
        {
            x.BearerTokenExpiration = TimeSpan.FromHours(1);
            x.RefreshTokenExpiration = TimeSpan.FromDays(14);
        });
    }
}
