using Backynet.Core;

namespace Backynet.Tests;

public class BackynetClientTests
{
    [Fact]
    public async Task Should_Execute_Job_When_Job_Was_Enqueued()
    {
        using var timeout = new CancellationTokenSource();
        timeout.CancelAfter(TimeSpan.FromSeconds(10));

        var backynetClient = new BackynetClient();
        await backynetClient.EnqueueAsync(() => FakeSyncMethod(), CancellationToken.None);

        WasExecuted.Wait(timeout.Token);
    }

    private static readonly ManualResetEventSlim WasExecuted = new();

    private static void FakeSyncMethod()
    {
        WasExecuted.Set();
    }
}