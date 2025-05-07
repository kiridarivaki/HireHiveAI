using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HireHive.Infrastructure.Startup
{
    public static class HangfireConfiguration
    {
        public static void ConfigureHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config =>
                config.UsePostgreSqlStorage(c =>
                c.UseNpgsqlConnection(configuration["HireHivePostgresConnectionString"])))
            .AddHangfireServer();
        }
    }
}
