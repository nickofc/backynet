using Backynet.Core;
using Backynet.Postgresql;

namespace Backynet.Tests;

public class BackynetClientTests
{
    public BackynetClientTests()
    {
        WasExecuted.Reset();
    }

    [Fact]
    public async Task Should_Execute_Action_Job_When_Job_Was_Enqueued()
    {
        // todo: move to sut? 
        using var timeout = new CancellationTokenSource();
        timeout.CancelAfter(TimeSpan.FromSeconds(1000));

        var factory = new NpgsqlConnectionFactory(TestContext.ConnectionString);
        var serializer = new DefaultJsonSerializer();
        var repository = new PostgreSqlRepository(factory, serializer);

        var backynetServer = new BackynetServer(repository, new SimpleJobRunner(), new BackynetServerOptions());
        await backynetServer.Start(CancellationToken.None);

        var backynetClient = new BackynetClient(repository);
        await backynetClient.EnqueueAsync(() => Console.WriteLine("hello world"), CancellationToken.None);

        WasExecuted.Wait(timeout.Token);
    }

    [Fact]
    public async Task Should_Execute_Func_Job_When_Job_Was_Enqueued()
    {
        using var timeout = new CancellationTokenSource();
        timeout.CancelAfter(TimeSpan.FromSeconds(1000));

        var factory = new NpgsqlConnectionFactory(TestContext.ConnectionString);
        var serializer = new DefaultJsonSerializer();
        var repository = new PostgreSqlRepository(factory, serializer);

        var backynetServer = new BackynetServer(repository, new SimpleJobRunner(), new BackynetServerOptions());
        await backynetServer.Start(CancellationToken.None);

        var backynetClient = new BackynetClient(repository);
        await backynetClient.EnqueueAsync(() => FakeSyncMethod(), CancellationToken.None);

        WasExecuted.Wait(timeout.Token);
    }

    private static readonly ManualResetEventSlim WasExecuted = new();

    private static Task FakeSyncMethod()
    {
        WasExecuted.Set();
        return Task.CompletedTask;
    }
}