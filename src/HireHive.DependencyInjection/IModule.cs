using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HireHive.DependencyInjection;

public interface IModule
{
    void ConfigureDependencyInjection(IServiceCollection services, IConfiguration configuration);
}
