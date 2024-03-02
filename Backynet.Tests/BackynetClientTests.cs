using Backynet.Core;

namespace Backynet.Tests;

public class BackynetClientTests
{
    [Fact]
    public async Task Should_Execute_Job_When_Job_Was_Enqueued()
    {
        var timeout = TimeSpan.FromSeconds(10);

        var backynetClient = new BackynetClient();
        await backynetClient.EnqueueAsync(() => FakeClass.FakeSyncMethod());

        if (!FakeClass.WasExecuted.WaitOne(timeout))
        {
            Assert.Fail("Timeout");
        }
    }

    private class FakeClass
    {
        public static readonly EventWaitHandle WasExecuted
            = new ManualResetEvent(false);

        public static void FakeSyncMethod()
        {
            WasExecuted.Set();
        }
    }
}