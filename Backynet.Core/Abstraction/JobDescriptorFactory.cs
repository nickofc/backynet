using System.Linq.Expressions;

namespace Backynet.Core.Abstraction;

public static class JobDescriptorFactory
{
    public static JobDescriptor CreateFromExpression(Expression expression)
    {
        if (expression is not LambdaExpression lambdaExpression)
        {
            throw new InvalidOperationException("Expression must be LambdaExpression.");
        }

        if (lambdaExpression.Body is not MethodCallExpression methodCallExpression)
        {
            throw new InvalidOperationException("Expression must be MethodCallExpression.");
        }

        var method = methodCallExpression.Method;

        if (!method.IsStatic)
        {
            throw new InvalidOperationException("Method must be static.");
        }

        var args = ResolveArgs(methodCallExpression);
        return new JobDescriptor(method.DeclaringType.AssemblyQualifiedName, method.Name, args);
    }

    private static object[] ResolveArgs(MethodCallExpression body)
    {
        var values = new object[body.Arguments.Count];

        for (var i = 0; i < body.Arguments.Count; i++)
        {
            var argument = body.Arguments[i];

            var expression = ResolveMemberExpression(argument);
            var value = GetValue(expression);

            values[i] = value;
        }

        return values;
    }

    private static MemberExpression ResolveMemberExpression(Expression expression)
    {
        if (expression is MemberExpression)
        {
            return (MemberExpression)expression;
        }

        if (expression is UnaryExpression)
        {
            // if casting is involved, Expression is not x => x.FieldName but x => Convert(x.Fieldname)
            return (MemberExpression)((UnaryExpression)expression).Operand;
        }

        throw new NotSupportedException(expression.ToString());
    }

    private static object GetValue(MemberExpression exp)
    {
        // expression is ConstantExpression or FieldExpression
        if (exp.Expression is ConstantExpression)
        {
            return (((ConstantExpression)exp.Expression).Value)
                .GetType()
                .GetField(exp.Member.Name)
                .GetValue(((ConstantExpression)exp.Expression).Value);
        }

        if (exp.Expression is MemberExpression)
        {
            return GetValue((MemberExpression)exp.Expression);
        }

        throw new NotImplementedException();
    }

    internal static class Empty
    {
        public static Task Method()
        {
            return Task.CompletedTask;
        }
    }
}