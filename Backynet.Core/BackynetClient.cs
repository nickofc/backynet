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

    public Task<string> EnqueueAsync(Expression<Func<Task>> expression, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);

        return EnqueueCoreAsync(expression, null, cancellationToken);
    }

    public Task<string> EnqueueAsync(Expression<Action> expression, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);

        return EnqueueCoreAsync(expression, null, cancellationToken);
    }

    public Task<string> EnqueueAsync(Expression<Func<Task>> expression, string groupName, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        ArgumentNullException.ThrowIfNull(groupName);

        return EnqueueCoreAsync(expression, groupName, cancellationToken);
    }

    public Task<string> EnqueueAsync(Expression<Action> expression, string groupName, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        ArgumentNullException.ThrowIfNull(groupName);

        return EnqueueCoreAsync(expression, groupName, cancellationToken);
    }

    private async Task<string> EnqueueCoreAsync(Expression expression, string? groupName = null, CancellationToken cancellationToken = default)
    {
        var job = Job.Create(expression);
        job.GroupName = groupName;
        job.NextOccurrenceAt = job.Cron != null
            ? DateTimeOffset.Now.Add(CronUtil.GetNextOccurrence(job.Cron))
            : DateTimeOffset.Now;
        job.JobState = JobState.Scheduled;
        await _storage.Add(job, cancellationToken);
        return job.Id.ToString();
    }
}