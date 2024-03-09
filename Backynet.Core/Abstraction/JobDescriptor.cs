using System.Linq.Expressions;

namespace Backynet.Core.Abstraction;

public readonly struct JobDescriptor : IJobDescriptor
{
    public string BaseType { get; }
    public string Method { get; }
    public object[] Arguments { get; }

    public JobDescriptor(string baseType, string method, object[] arguments)
    {
        BaseType = baseType;
        Method = method;
        Arguments = arguments;
    }

    public static JobDescriptor Empty()
    {
        return JobDescriptorFactory.CreateFromExpression(() => JobDescriptorFactory.Empty.Method());
    }

    public static JobDescriptor Create(Expression expression)
    {
        return JobDescriptorFactory.CreateFromExpression(expression);
    }
}