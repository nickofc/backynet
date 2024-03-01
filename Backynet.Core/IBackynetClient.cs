using System.Linq.Expressions;

namespace Backynet.Core;

public interface IBackynetClient
{
    Task<string> EnqueueAsync(Expression<Func<Task>> call);
    Task<string> EnqueueAsync(Expression<Action> call);
    
    Task<string> ContinueWithAsync(string jobId, Expression<Func<Task>> call);
    Task<string> ContinueWithAsync(string jobId, Expression<Action> call);
    
    Task DeleteAsync(string jobId);
}