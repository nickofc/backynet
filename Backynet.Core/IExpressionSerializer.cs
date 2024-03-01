using System.Linq.Expressions;

namespace Backynet.Core;

internal interface IExpressionSerializer
{
    Stream Serialize(Expression<Func<Task>> call);
}