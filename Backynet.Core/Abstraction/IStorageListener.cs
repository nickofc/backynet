namespace Backynet.Core.Abstraction;

public interface IStorageListener<out T>
{
    Task Start(Func<T, Task> callback, CancellationToken cancellationToken);
}