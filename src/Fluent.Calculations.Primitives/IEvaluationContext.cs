namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

public interface IEvaluationContext<T> where T : class, IValueProvider, new()
{
    TValue Evaluate<TValue>(
        Expression<Func<TValue>> lambdaExpression,
        [CallerMemberName] string name = StringConstants.NaN,
        [CallerArgumentExpression("lambdaExpression")] string lambdaExpressionBody = StringConstants.NaN) 
        where TValue : class, IValueProvider, new();

    T ToResult();
}