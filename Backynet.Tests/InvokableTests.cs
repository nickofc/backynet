using System.Linq.Expressions;
using Backynet.Core;

namespace Backynet.Tests;

public class InvokableTests
{
    [Fact]
    public void Should_Create_Invokable_When_Invoked_With_Valid_Expression()
    {
        var stringArgument = "abcde";
        var classArgument = new FakeDto { Username = "Antoni" };

        Expression<Func<Task>> expression = () => FakeAsyncMethod(stringArgument, classArgument);
        var invokable = Invokable.Create(expression);

        Assert.NotNull(invokable.Method);
        Assert.NotNull(invokable.ReturnType);
        Assert.NotNull(invokable.Arguments);
    }

    public static Task FakeAsyncMethod(string fakeArg1, FakeDto fakeDto)
    {
        return Task.CompletedTask;
    }

    public static void FakeSyncMethod(string fakeArg1, FakeDto fakeDto)
    {
    }

    public class FakeDto
    {
        public string Username { get; set; }
    }
}