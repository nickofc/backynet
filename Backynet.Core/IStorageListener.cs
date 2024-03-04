namespace Backynet.Core;

public interface IStorageListener
{
    event EventHandler OnItemAdded;
    Task Start(CancellationToken cancellationToken);
}