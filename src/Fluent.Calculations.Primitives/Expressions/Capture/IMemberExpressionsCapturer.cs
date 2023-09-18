namespace Fluent.Calculations.Primitives.Expressions.Capture;
using System.Linq.Expressions;

internal interface IMemberExpressionsCapturer
{
    List<MemberExpression> Capture(Expression expression);
}