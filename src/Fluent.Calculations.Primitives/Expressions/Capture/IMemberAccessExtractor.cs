namespace Fluent.Calculations.Primitives.Expressions.Capture;
using System.Linq.Expressions;

internal interface IMemberAccessExtractor
{
    List<object> CaptureParameterMember(MemberExpression expression);
}