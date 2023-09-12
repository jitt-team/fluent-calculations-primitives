namespace Fluent.Calculations.Primitives.Expressions.Capture;
using System.Linq.Expressions;

public interface IExpressionMembersCapturer
{
    List<object> Capture(Expression expression);
}