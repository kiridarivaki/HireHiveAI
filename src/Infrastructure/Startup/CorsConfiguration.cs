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
                    policy.WithOrigins("http://localhost:4200")
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
