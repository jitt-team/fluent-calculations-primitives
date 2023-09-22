namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;

public interface IMemberExpressionValueCapturer
{
    MemberExpressionValues Capture<TExpressionResulValue>(Expression<Func<TExpressionResulValue>> expression) where TExpressionResulValue : class, IValue;
}