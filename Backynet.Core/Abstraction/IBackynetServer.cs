namespace Backynet.Core.Abstraction;

public interface IBackynetServer
{
    Task Start(CancellationToken cancellationToken);
}