using System.Linq.Expressions;
using Backynet.Core;
using Backynet.Core.Abstraction;

namespace Backynet.Tests;

public class SimpleJobRunnerTest
{
    [Fact]
    public async Task Should_Execute_Invokable()
    {
        var valueTypeArgument = 997;
        var referenceTypeArgument = new FakeDto { Username = "Antoni" };

        Expression<Func<Task>> expression = () => FakeAsyncMethod(valueTypeArgument, referenceTypeArgument);
        var job = Job.Create(expression);

        var jobRunner = new SimpleJobRunner();
        await jobRunner.Run(job);

        Assert.True(WasExecuted.IsSet);
    }

    private static readonly ManualResetEventSlim WasExecuted = new();

    private static Task FakeAsyncMethod(int fakeArg1, FakeDto fakeArg2)
    {
        WasExecuted.Set();
        return Task.CompletedTask;
    }

    private record FakeDto
    {
        public string Username { get; set; }
    }
}