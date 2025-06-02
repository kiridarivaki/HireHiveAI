using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HireHive.Infrastructure.Startup
{
    public static class CorsConfiguration
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", policy =>
                {
                    policy.WithOrigins("https://wonderful-flower-0782c0b03.6.azurestaticapps.net/")
                         .AllowAnyHeader()
                     .AllowAnyMethod();
                });
            });
        }

        public static void EnableCors(this WebApplication app)
        {
            app.UseCors("AllowSpecificOrigins");
        }
    }
}
