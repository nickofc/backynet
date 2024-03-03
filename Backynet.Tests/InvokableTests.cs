using System.Linq.Expressions;
using Backynet.Core;
using Newtonsoft.Json;

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
        var classArgument = new FakeDto { Username = "Antoni", Age = 33 };

        Expression<Func<Task>> expression = () => FakeAsyncMethod(valueTypeArgument, classArgument);
        var expectedInvokable = Invokable.GetFromExpression(expression);

        // todo: uzyć wydajnego serializera

        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        var payload = JsonConvert.SerializeObject(expectedInvokable, settings);
        var actualInvokable = JsonConvert.DeserializeObject<Invokable>(payload, settings);

        Assert.Equal(expectedInvokable.Method, actualInvokable.Method);
        // https://stackoverflow.com/a/9444519/6210759
        Assert.Equal(expectedInvokable.Arguments[0], Convert.ToInt32(actualInvokable.Arguments[0]));
        Assert.Equal(expectedInvokable.Arguments[1], actualInvokable.Arguments[1]);
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
        public int Age { get; set; }
    }
}