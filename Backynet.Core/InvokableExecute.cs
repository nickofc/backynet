using System.Reflection;

namespace Backynet.Core;

internal class InvokableExecute
{
    // TODO: ioc
    // TODO: cache?

    public async Task ExecuteAsync(Invokable invokable)
    {
        var member = Type.GetType(invokable.BaseType);
        var methodInfo = member.GetMethod(invokable.Method, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        var invokeReturnValue = methodInfo.Invoke(null, invokable.Arguments.ToArray());

        if (invokeReturnValue is Task t)
        {
            await t;
        }
    }
}