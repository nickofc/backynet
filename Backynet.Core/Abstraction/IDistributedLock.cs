namespace Backynet.Core.Abstraction;

public interface IDistributedLock
{
    Task<bool> TryAcquire(string key);
}