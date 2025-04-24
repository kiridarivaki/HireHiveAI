using FluentValidation;
using HireHive.DependencyInjection;
using System.Reflection;

namespace HireHive.Api
{
    public class ApiModule : IModule
    {
        public void ConfigureDependencyInjection(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);
        }
    }
}
