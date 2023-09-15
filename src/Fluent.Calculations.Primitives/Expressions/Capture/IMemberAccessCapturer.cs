namespace Fluent.Calculations.Primitives.Expressions.Capture;
using System.Linq.Expressions;

internal interface IMemberAccessCapturer
{
    List<object> Capture(MemberExpression expression);
}