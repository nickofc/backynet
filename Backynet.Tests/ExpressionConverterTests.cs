using System.Linq.Expressions;

namespace Backynet.Tests;

public class ExpressionConverterTests
{
    [Fact]
    public void Test1()
    {
        var arg = "abcde"; 
        var fakeDto = new FakeDto { Username = "wows" };
        var instance = new FakeClass();
        
        Expression<Func<Task>> expression = ()
            => instance.FakeMethodAsync(arg, fakeDto);
        
        var call = expression.Body as MethodCallExpression;

        var method = call.Method;
        var args = call.Arguments;
    }
    
    public class FakeClass
    {
        public Task FakeMethodAsync(string fakeArg1, FakeDto fakeDto)
        {
            return Task.CompletedTask;
        }

        public void FakeMethod(string fakeArg1, FakeDto fakeDto)
        {
        }
    }

    public class FakeDto
    {
        public string Username { get; set; }
    }
}