namespace Backynet.Core;

public interface IDistributedLock
{
    Task<bool> TryAcquire(string key);
}