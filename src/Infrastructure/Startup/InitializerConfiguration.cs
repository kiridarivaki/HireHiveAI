using HireHive.Domain.Entities;
using HireHive.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HireHive.Infrastructure.Startup
{
    public static class InitializerConfiguration
    {
        public static async void Seed(this IServiceProvider services, IConfiguration configuration)
        {
            var scopeFactory = (IServiceScopeFactory)services.GetService(typeof(IServiceScopeFactory))!;

            using var scope = scopeFactory.CreateScope();
            var provider = scope.ServiceProvider;

            var userManager = provider.GetRequiredService<UserManager<User>>();
            var context = provider.GetRequiredService<AppDbContext>();
            await Initializer.Seed(context, userManager, configuration);
        }
    }

}
