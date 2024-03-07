namespace Backynet.Core.Abstraction;

public interface IStorage
{
    Task Add(Job job, CancellationToken cancellationToken = default);
    Task<Job?> Get(Guid jobId, CancellationToken cancellationToken = default);
}