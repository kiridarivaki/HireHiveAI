using Application.Interfaces;
using Application.Services;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddAutoMapper(assembly);
        //services.AddValidatorsFromAssembly(assembly);
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
