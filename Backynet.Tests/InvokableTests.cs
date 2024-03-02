using System.Linq.Expressions;
using System.Text.Json;
using Backynet.Core;

namespace Backynet.Tests;

public class InvokableTests
{
    [Fact]
    public void Should_Create_Invokable_When_Invoked_With_Valid_Expression()
    {
        var valueTypeArgument = 997;
        var referenceTypeArgument = new FakeDto { Username = "Antoni" };

        Expression<Func<Task>> expression = () => FakeAsyncMethod(valueTypeArgument, referenceTypeArgument);
        var invokable = Invokable.GetFromExpression(expression);

        Assert.NotNull(invokable.Method);
        Assert.NotNull(invokable.Arguments);

        Assert.Equal(valueTypeArgument, invokable.Arguments[0]);
        Assert.Equal(referenceTypeArgument, invokable.Arguments[1]);
    }

    [Fact]
    public void Should_Serialize_Invokable()
    {
        var valueTypeArgument = 997;
        var classArgument = new FakeDto { Username = "Antoni" };

        Expression<Func<Task>> expression = () => FakeAsyncMethod(valueTypeArgument, classArgument);
        var expectedInvokable = Invokable.GetFromExpression(expression);

        var payload = JsonSerializer.Serialize(expectedInvokable);
        var actualInvokable = JsonSerializer.Deserialize<Invokable>(payload);
        
        Assert.Equal(expectedInvokable.Method, actualInvokable.Method);
        Assert.Equal(expectedInvokable.Arguments, actualInvokable.Arguments);
    }

    private static Task FakeAsyncMethod(int fakeArg1, FakeDto fakeArg2)
    {
        return Task.CompletedTask;
    }

    private static void FakeSyncMethod(int fakeArg1, FakeDto fakeArg2)
    {
    }

    private record FakeDto
    {
        public string Username { get; set; }
    }
}