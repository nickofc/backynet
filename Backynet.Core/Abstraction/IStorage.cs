namespace Backynet.Core.Abstraction;

public interface IStorage
{
    Task Add(Job job, CancellationToken cancellationToken = default);
    Task Delete(string jobId, CancellationToken cancellationToken = default);
}