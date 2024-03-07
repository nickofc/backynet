using System.Linq.Expressions;
using Backynet.Core.Abstraction;

namespace Backynet.Tests;

public class JobTests
{
    [Fact]
    public void Should_Return_New_Job_When_Create_Is_Called_With_Valid_Arguments()
    {
        // arrange

        var valueTypeArgument = 997;
        var referenceTypeArgument = new FakeDto { Username = "Antoni" };
        Expression<Func<Task>> expression = () => FakeAsyncMethod(valueTypeArgument, referenceTypeArgument);

        // act

        var job = Job.Create(expression);

        // assert

        Assert.NotNull(job.Descriptor.Method);
        Assert.NotNull(job.Descriptor.Arguments);
        Assert.Equal(valueTypeArgument, job.Descriptor.Arguments[0]);
        Assert.Equal(referenceTypeArgument, job.Descriptor.Arguments[1]);
    }

    private static Task FakeAsyncMethod(int fakeArg1, FakeDto fakeArg2)
    {
        return Task.CompletedTask;
    }

    private record FakeDto
    {
        public string Username { get; set; }
        public int Age { get; set; }
    }
}