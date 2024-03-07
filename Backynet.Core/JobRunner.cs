using System.Reflection;
using Backynet.Core.Abstraction;

namespace Backynet.Core;

internal class JobRunner
{
    // TODO: ioc
    // TODO: cache?

    public async Task RunAsync(IJobDescriptor jobDescriptor)
    {
        var member = Type.GetType(jobDescriptor.BaseType);
        var methodInfo = member.GetMethod(jobDescriptor.Method, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        var invokeReturnValue = methodInfo.Invoke(null, jobDescriptor.Arguments);

        if (invokeReturnValue is Task t)
        {
            await t;
        }
    }
}