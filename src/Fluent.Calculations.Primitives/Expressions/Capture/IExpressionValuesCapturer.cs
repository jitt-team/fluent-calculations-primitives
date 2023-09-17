using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;

namespace Fluent.Calculations.Primitives.Expressions.Capture
{
    internal interface IExpressionValuesCapturer
    {
        CapturedExpressionValues Capture<TExpressionResulValue>(Expression<Func<TExpressionResulValue>> expression) where TExpressionResulValue : class, IValue;
    }
}