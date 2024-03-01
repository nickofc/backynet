using Backynet.Core;
using Microsoft.Extensions.Hosting;

namespace Backynet.AspNetCore;

internal sealed class BackynetServerHostedService : BackgroundService
{
    private readonly IBackynetServer _backynetServer;

    public BackynetServerHostedService(IBackynetServer backynetServer)
    {
        _backynetServer = backynetServer;
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        return _backynetServer.Start(cancellationToken);
    }
}