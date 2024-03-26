using Backynet.Core.Abstraction;

namespace Backynet.Core;

internal sealed class BackynetServer : IBackynetServer
{
    private readonly IStorage _storage;
    private readonly IJobRunner _jobRunner;
    private readonly BackynetServerOptions _backynetServerOptions;

    public BackynetServer(IStorage storage, IJobRunner jobRunner, BackynetServerOptions backynetServerOptions)
    {
        _storage = storage;
        _jobRunner = jobRunner;
        _backynetServerOptions = backynetServerOptions;
    }

    public  Task Start(CancellationToken cancellationToken)
    {
        var task = StartCore(cancellationToken);
        return task.IsCompleted ? task : Task.CompletedTask;
    }

    private async Task StartCore(CancellationToken cancellationToken)
    {
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var jobs = await _storage.Acquire(_backynetServerOptions.ServerName, 1, cancellationToken);

            foreach (var job in jobs)
            {
                try
                {
                    await _jobRunner.Run(job);
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
