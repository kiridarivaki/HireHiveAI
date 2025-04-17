using Application.Interfaces;
using Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        //services.AddMediatR(configuration =>
        //        configuration.RegisterServicesFromAssembly(assembly));
        
        services.AddValidatorsFromAssembly(assembly);
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //added 
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
