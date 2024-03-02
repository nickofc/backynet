using System.Linq.Expressions;
using Backynet.Core;

namespace Backynet.Tests;

public class InvokableExecuteTest
{
    [Fact]
    public async Task Should_Execute_Invokable()
    {
        var valueTypeArgument = 997;
        var referenceTypeArgument = new FakeDto { Username = "Antoni" };

        Expression<Func<Task>> expression = () => FakeAsyncMethod(valueTypeArgument, referenceTypeArgument);
        var invokable = Invokable.GetFromExpression(expression);

        var invokableExecute = new InvokableExecute();
        await invokableExecute.ExecuteAsync(invokable);
        
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