using Backynet.Core;

namespace Backynet.Tests;

public class BackynetClientTests
{
    [Fact]
    public async Task Unit()
    {
        var timeout = TimeSpan.FromSeconds(10);
        var classStub = new ClassStub();

        var backynetClient = new BackynetClient();
        await backynetClient.EnqueueAsync(() => classStub.Execute());

        if (!ClassStub.WasExecuted.WaitOne(timeout))
        {
            Assert.Fail("Timeout");
        }
    }

    public class ClassStub
    {
        public static readonly EventWaitHandle WasExecuted 
            = new ManualResetEvent(false);

        public void Execute()
        {
            WasExecuted.Set();
        }
    }
}