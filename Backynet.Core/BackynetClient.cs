using System.Linq.Expressions;

namespace Backynet.Core;

public class BackynetClient : IBackynetClient
{
    public Task<string> EnqueueAsync(Expression<Func<Task>> call)
    {
        throw new NotImplementedException();
    }

    public Task<string> EnqueueAsync(Expression<Action> call)
    {
        throw new NotImplementedException();
    }

    public Task<string> ContinueWithAsync(string jobId, Expression<Func<Task>> call)
    {
        throw new NotImplementedException();
    }

    public Task<string> ContinueWithAsync(string jobId, Expression<Action> call)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string jobId)
    {
        throw new NotImplementedException();
    }
}