using Backynet.Core.Abstraction;

namespace Backynet.Core;

public class BackynetServer : IBackynetServer
{
    public BackynetServer(BackynetServerOptions options)
    {
        throw new NotImplementedException();
    }

    public Task Start(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}