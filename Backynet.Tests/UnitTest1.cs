using System.Linq.Expressions;
using System.Text.Json;
using System.Xml.Serialization;

namespace Backynet.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var arg = "abcde";
        var instance = new Method();
        
        Expression<Func<Task>> expression = ()
            => instance.Execute(arg);
        
        var call = expression.Body as MethodCallExpression;

        var method = call.Method;
        var args = call.Arguments;

    }
}

public class Method
{
    public Task Execute(string arg)
    {
        return Task.CompletedTask;
    }
}