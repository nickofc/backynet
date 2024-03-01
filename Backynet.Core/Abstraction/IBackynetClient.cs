using System.Linq.Expressions;

namespace Backynet.Core.Abstraction;

public interface IBackynetClient
{
    Task<string> EnqueueAsync(Expression<Func<Task>> call, CancellationToken cancellationToken = default);
    Task<string> EnqueueAsync(Expression<Action> call, CancellationToken cancellationToken = default);
}