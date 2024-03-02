namespace Backynet.Core.Abstraction;

public interface IStorage
{
    Task Add(Job job, CancellationToken cancellationToken = default);
}