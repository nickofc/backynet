using System.Transactions;
using Backynet.Core.Abstraction;

namespace Backynet.Core;

internal sealed class BackynetServer : IBackynetServer
{
    private readonly IStorage _storage;

    public BackynetServer(IStorage storage)
    {
        _storage = storage;
    }

    public  Task Start(CancellationToken cancellationToken)
    {
        var t = StartCore(cancellationToken);
        return t.IsCompleted ? t : Task.CompletedTask;
    }

    private async Task StartCore(CancellationToken cancellationToken)
    {
        var serverName = "Admin-PC";
        var jobRunner = new JobRunner();

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var jobs = await _storage.Acquire(serverName, 1, cancellationToken);

            foreach (var job in jobs)
            {
                try
                {
                    await jobRunner.RunAsync(job.Descriptor);
                }
                catch (Exception e)
                {
                    job.JobState = JobState.Failed;
                    await _storage.Update(job.Id, job, cancellationToken);
                }
            }

            await Task.Delay(1000, cancellationToken);
        }
    }
}