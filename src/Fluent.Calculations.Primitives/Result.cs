namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

/// <include file="IntelliSense.xml" path='*/Result/class/*' />
public static class Result
{
    public static TValue Of<TValue>(Expression<Func<TValue>> lambdaExpression,
    [CallerMemberName] string name = StringConstants.NaN,
    [CallerArgumentExpression(nameof(lambdaExpression))] string lambdaExpressionBody = StringConstants.NaN) where TValue : class, IValueProvider, new() =>
        OfWithScope(lambdaExpression, StringConstants.NaN, name, lambdaExpressionBody);

    public static TValue OfWithScope<TValue>(Expression<Func<TValue>> lambdaExpression,
        string scopeName,
        [CallerMemberName] string name = StringConstants.NaN,
        [CallerArgumentExpression(nameof(lambdaExpression))] string lambdaExpressionBody = StringConstants.NaN) where TValue : class, IValueProvider, new()
    {
        TValue result = new EvaluationScope<TValue>(scopeName).Evaluate(lambdaExpression, name, lambdaExpressionBody);
        return (TValue)((IOrigin)result).AsResult();
    }
}
