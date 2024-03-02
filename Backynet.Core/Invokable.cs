using System.Linq.Expressions;

namespace Backynet.Core;

internal class Invokable
{
    public string BaseType {get; }
    public string Method { get; }
    public IReadOnlyList<object> Arguments { get; }

    public Invokable(string baseType, string method, IReadOnlyList<object> arguments)
    {
        BaseType = baseType;
        Method = method;
        Arguments = arguments;
    }

    public static Invokable GetFromExpression(Expression<Func<Task>> s)
    {
        return GetFromExpression((Expression)s);
    }

    public static Invokable GetFromExpression(Expression expression)
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

        return new Invokable(method.DeclaringType.AssemblyQualifiedName, method.Name, args);
    }

    private static IReadOnlyList<object> ResolveArgs(MethodCallExpression body)
    {
        var values = new List<object>(body.Arguments.Count);

        foreach (var argument in body.Arguments)
        {
            var expression = ResolveMemberExpression(argument);
            var value = GetValue(expression);

            values.Add(value);
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
}