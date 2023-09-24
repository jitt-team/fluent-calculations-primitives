namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;

public interface IMemberExpressionValueCapturer
{
    MemberExpressionMembers CaptureMembers<TExpressionResulValue>(Expression<Func<TExpressionResulValue>> lambdaExpression) where TExpressionResulValue : class, IValue;
}