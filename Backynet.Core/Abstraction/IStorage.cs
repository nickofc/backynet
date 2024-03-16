namespace Backynet.Core.Abstraction;

public interface IStorage
{
    Task Add(Job job, CancellationToken cancellationToken = default);
    Task<Job?> Get(Guid jobId, CancellationToken cancellationToken = default);
    Task Update(Guid jobId, Job job, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Job>> Acquire(string serverName, int count, CancellationToken cancellationToken = default);
}