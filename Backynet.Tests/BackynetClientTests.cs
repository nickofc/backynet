using Backynet.Core;
using Backynet.Postgresql;

namespace Backynet.Tests;

public class BackynetClientTests
{
    [Fact]
    public async Task Should_Execute_Job_When_Job_Was_Enqueued()
    {
        using var timeout = new CancellationTokenSource();
        timeout.CancelAfter(TimeSpan.FromSeconds(10));
        
        var factory = new NpgsqlConnectionFactory(TestContext.ConnectionString);
        var serializer = new DefaultJsonSerializer();
        var repository = new PostgreSqlRepository(factory, serializer);

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