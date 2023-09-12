namespace Fluent.Calculations.Primitives.Expressions.Capture;
using System.Linq.Expressions;

internal interface IExpressionMembersCapturer
{
    List<object> Capture(Expression expression);
}