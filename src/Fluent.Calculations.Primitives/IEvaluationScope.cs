namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

/// <include file="Docs/IntelliSense.xml" path='docs/members[@name="IEvaluationScope"]/interface/*' />
public interface IEvaluationScope
{
    TValue Evaluate<TValue>(
        Expression<Func<TValue>> lambdaExpression,
        [CallerMemberName] string name = StringConstants.NaN,
        [CallerArgumentExpression("lambdaExpression")] string lambdaExpressionBody = StringConstants.NaN)
        where TValue : class, IValueProvider, new();
}