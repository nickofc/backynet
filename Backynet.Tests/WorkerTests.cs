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

    [Fact]
    public async Task Should_Throw_Exception_When_Provided_Cancellation_Token_Is_Cancelled()
    {
        const string data = "hello-world";

        var channel = Channel.CreateBounded<string>(1);
        await channel.Writer.WriteAsync(data);

        var worker = new Worker<string>(channel.Reader, (_, _) => Task.Delay(-1, CancellationToken.None));
        await worker.Start();

        using var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(1));

        await Assert.ThrowsAsync<TaskCanceledException>(async () => { await worker.Stop(cts.Token); });
    }

    [Fact(Timeout = 1000)]
    public async Task Should_Cancel_Internal_Token_When_Stop_Is_Called()
    {
        const string data = "hello-world";

        var channel = Channel.CreateBounded<string>(1);
        await channel.Writer.WriteAsync(data);

        var worker = new Worker<string>(channel.Reader, (_, cancellationToken) => Task.Delay(-1, cancellationToken));
        await worker.Start();

        await worker.Stop(CancellationToken.None);
    }
}