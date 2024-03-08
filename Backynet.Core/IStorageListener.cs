namespace Backynet.Core;

public interface IStorageListener
{
    event EventHandler<string> OnItemAdded;
    Task Start(CancellationToken cancellationToken);
}