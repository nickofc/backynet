using System.Threading.Channels;

namespace Backynet.Core;

public class WorkerPool
{
    public int TaskCount { get; }

    public WorkerPool(int taskCount)
    {
        TaskCount = taskCount;
    }

    private CancellationTokenSource _cts;
    private Task _task;

    public async Task Start(CancellationToken cancellationToken)
    {
    }

    public async Task Stop()
    {
    }
}