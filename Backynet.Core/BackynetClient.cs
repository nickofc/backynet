using System.Linq.Expressions;
using Backynet.Core.Abstraction;

namespace Backynet.Core;

internal sealed class BackynetClient : IBackynetClient
{
    private readonly IStorage _storage;

    public BackynetClient(IStorage storage)
    {
        _storage = storage;
    }

    public async Task<string> EnqueueAsync(Expression<Func<Task>> expression, CancellationToken cancellationToken = default)
    {
        var job = Job.Create(expression);
        await _storage.Add(job, cancellationToken);
        return job.Id.ToString();
    }

    public Task<string> EnqueueAsync(Expression<Action> expression, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}