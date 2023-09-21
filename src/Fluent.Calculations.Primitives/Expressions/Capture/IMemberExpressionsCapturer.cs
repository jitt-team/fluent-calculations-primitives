namespace Fluent.Calculations.Primitives.Expressions.Capture;
using System.Linq.Expressions;

public interface IMemberExpressionsCapturer
{
    List<MemberExpression> Capture(Expression expression);
}