using System.Linq.Expressions;
using System.Xml;

namespace Backynet.Core;

internal class ExpressionConverter
{
    public Stream Serialize(Expression call)
    {
        if (call is not LambdaExpression lambdaExpression)
        {
            throw null;
        }

        var args = new Dictionary<object, object>();
        var method = "";

        var parma = lambdaExpression.Body;


        var test = parma as MethodCallExpression;
        
        throw new NotImplementedException();
    }
}