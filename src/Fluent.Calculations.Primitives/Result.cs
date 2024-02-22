namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

/// <include file="Docs.xml" path='*/Result/class/*'/>
public static class Result
{
    /// <include file="Docs.xml" path='*/Result/Of/*'/>
    public static TValue Of<TValue>(Expression<Func<TValue>> lambdaExpression,
    [CallerMemberName] string name = StringConstants.NaN,
    [CallerArgumentExpression(nameof(lambdaExpression))] string lambdaExpressionBody = StringConstants.NaN) where TValue : class, IValueProvider, new() =>
        OfWithScope(lambdaExpression, StringConstants.NaN, name, lambdaExpressionBody);

    /// <include file="Docs.xml" path='*/Result/OfWithScope/*'/>
    public static TValue OfWithScope<TValue>(Expression<Func<TValue>> lambdaExpression,
        string scopeName,
        [CallerMemberName] string name = StringConstants.NaN,
        [CallerArgumentExpression(nameof(lambdaExpression))] string lambdaExpressionBody = StringConstants.NaN) where TValue : class, IValueProvider, new()
    {
        TValue result = new EvaluationScope(scopeName).Evaluate(lambdaExpression, name, lambdaExpressionBody);
        return (TValue)((IOrigin)result).AsResult();
    }
}
