namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

public static class Result
{
    public static TValue Of<TValue>(Expression<Func<TValue>> lambdaExpression,
        [CallerMemberName] string name = StringConstants.NaN,
        [CallerArgumentExpression("lambdaExpression")] string lambdaExpressionBody = StringConstants.NaN) where TValue : class, IValue, new()
    {
        TValue result = new EvaluationContext<TValue>().Evaluate(lambdaExpression, name, lambdaExpressionBody);
        return (TValue)((IOrigin)result).AsResult();
    }
}
