using Backynet.Core;

namespace Backynet.Tests;

public class WorkerPoolTests
{
    [Fact]
    public async Task Should_Start()
    {
        const int initialCount = 10000;
        
        using var countdownEvent = new CountdownEvent(initialCount);
        var workerPool = new WorkerPool<int>(10, (_, _) =>
        {
            countdownEvent.Signal();
            return Task.CompletedTask;
        });
        await workerPool.Start(CancellationToken.None);
        
        for (var i = 0; i < initialCount; i++)
        {
            await workerPool.Post(i);
        }

        using var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(10));
        
        countdownEvent.Wait(cts.Token);
    }
}