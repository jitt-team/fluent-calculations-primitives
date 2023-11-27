namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

public static class Result
{
    public static TValue Of<TValue>(Expression<Func<TValue>> lambdaExpression,
        [CallerMemberName] string name = StringConstants.NaN,
        [CallerArgumentExpression(nameof(lambdaExpression))] string lambdaExpressionBody = StringConstants.NaN) where TValue : class, IValueProvider, new()
    {
        TValue result = new EvaluationContext<TValue>().Evaluate(lambdaExpression, name, lambdaExpressionBody);
        return (TValue)((IOrigin)result).AsResult();
    }
}
