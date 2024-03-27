using System.Linq.Expressions;

namespace Backynet.Core.Abstraction;

public interface IBackynetClient
{
    Task<string> EnqueueAsync(Expression<Func<Task>> expression, CancellationToken cancellationToken = default);
    Task<string> EnqueueAsync(Expression<Action> expression, CancellationToken cancellationToken = default);
    Task<string> EnqueueAsync(Expression<Func<Task>> expression, string groupName, CancellationToken cancellationToken = default);
    Task<string> EnqueueAsync(Expression<Action> expression, string groupName, CancellationToken cancellationToken = default);
}