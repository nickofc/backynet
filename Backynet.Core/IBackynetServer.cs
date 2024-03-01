namespace Backynet.Core;

public interface IBackynetServer
{
    Task Start(CancellationToken cancellationToken);
}