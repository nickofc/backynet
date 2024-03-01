using Backynet.Postgresql;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static BackynetConfigurationBuilder UsePostgreSql(
        this BackynetConfigurationBuilder backynetConfigurationBuilder,
        Action<BackynetPosgresqlServerOptions> configure)
    {
        return backynetConfigurationBuilder;
    }
}