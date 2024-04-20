﻿using Backynet.AspNetCore;
using Backynet.Core;
using Backynet.Core.Abstraction;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddBackynetClient(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddBackynetServer(this IServiceCollection services,
        Action<BackynetServerOptionsBuilder> configure)
    {
        var builder = new BackynetServerOptionsBuilder(services);

        configure(builder);

        return services;
    }

    public static IServiceCollection AddBackynetServer(this IServiceCollection services,
        Action<IServiceProvider, BackynetServerOptions> configure)
    {
        services.AddHostedService<BackynetServerHostedService>(sp =>
        {
            var options = new BackynetServerOptions();
            configure.Invoke(sp, options);

            var jobRepository = sp.GetRequiredService<IJobRepository>();
            var jobExecutor = sp.GetRequiredService<IJobExecutor>();
            var serverService = sp.GetRequiredService<IServerService>();
            var threadPool = sp.GetRequiredService<IThreadPool>();
            var backynetServer = new BackynetServer(jobRepository, jobExecutor, options, serverService, threadPool);

            return new BackynetServerHostedService(backynetServer);
        });

        return services;
    }
}

public class BackynetServerOptionsBuilder
{
    public IServiceCollection Services { get; }

    public BackynetServerOptionsBuilder(IServiceCollection services)
    {
        Services = services;
    }

    internal BackynetServerOptions Options { get; } = new();

    public BackynetServerOptionsBuilder UseMaximumConcurrencyThreads(int maximumConcurrencyThreads)
    {
        Options.MaxThreads = maximumConcurrencyThreads;
        return this;
    }

    public BackynetServerOptionsBuilder UseServerName(string serverName)
    {
        Options.ServerName = serverName;
        return this;
    }
}