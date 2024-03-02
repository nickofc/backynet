using System.Linq.Expressions;

namespace Backynet.Core;

internal class Invokable
{
    public string Method { get; }
    public IReadOnlyList<object> Arguments { get; }
    public string ReturnType { get; }

    public Invokable(string method, IReadOnlyList<object> arguments, string returnType)
    {
        Method = method;
        Arguments = arguments;
        ReturnType = returnType;
    }

    public static Invokable Create(Expression<Func<Task>> s)
    {
        return Create((Expression) s);
    }
    
    public static Invokable Create(Expression expression)
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

        return new Invokable(method.Name, args, method.ReturnType.Name);
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

    public static MemberExpression ResolveMemberExpression(Expression expression)
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
        else if (exp.Expression is MemberExpression)
        {
            return GetValue((MemberExpression)exp.Expression);
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    public ValueTask Invoke()
    {
        return ValueTask.CompletedTask;
    }
}