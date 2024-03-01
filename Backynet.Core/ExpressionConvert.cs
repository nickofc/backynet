using System.Linq.Expressions;
using System.Xml;

namespace Backynet.Core;

public class ExpressionSerializer
{
    public Stream Serialize(Expression expression)
    {
        return Stream.Null;
    }

    public Expression Deserialize(Stream stream)
    {
        return null;
    } 
}