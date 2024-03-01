using Backynet.AspNetCore;
using Backynet.Core;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddBackynetServer(this IServiceCollection services, 
        Action<BackynetConfigurationBuilder> configure)
    {
        return services;
    }
    
    public static IServiceCollection AddBackynetServer(this IServiceCollection services, 
        Action<IServiceProvider, BackynetServerOptions> configure)
    {
        services.AddHostedService<BackynetServerHostedService>(sp =>
        {
            var options = new BackynetServerOptions();
            configure.Invoke(sp, options);

            var server = new BackynetServer(options);

            return new BackynetServerHostedService(server);
        });

        return services;
    }
}