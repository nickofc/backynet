using System.Threading.Channels;
using Backynet.Core;

namespace Backynet.Tests;

public class WorkerTests
{
    [Fact]
    public async Task Should_Invoke_Func_When_Something_Writes_To_Channel()
    {
        var manualResetEventSlim = new ManualResetEventSlim();
        var channel = Channel.CreateBounded<string>(1);
        var worker = new Worker<string>(channel.Reader, (a, b) =>
        {
            manualResetEventSlim.Set();
            return Task.CompletedTask;
        });

        await channel.Writer.WriteAsync("hello-world");
        await worker.Start();

        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(10));
        manualResetEventSlim.Wait(cts.Token);
    }

    [Fact]
    public async Task Should_Stop_Without_Any_Exception_When_Invoked_Stop()
    {
        var manualResetEventSlim = new ManualResetEventSlim();
        var channel = Channel.CreateBounded<string>(1);
        var worker = new Worker<string>(channel.Reader, (a, b) =>
        {
            manualResetEventSlim.Set();
            return Task.CompletedTask;
        });

        await channel.Writer.WriteAsync("hello-world");
        await worker.Start();

        await worker.Stop();
    }
}