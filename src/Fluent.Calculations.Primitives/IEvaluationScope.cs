namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

/// <include file="Docs.xml" path='*/IEvaluationScope/interface/*'/>
public interface IEvaluationScope
{
    /// <include file="Docs.xml" path='*/IEvaluationScope/Evaluate/*'/>
    TValue Evaluate<TValue>(
        Expression<Func<TValue>> lambdaExpression,
        [CallerMemberName] string name = StringConstants.NaN,
        [CallerArgumentExpression(nameof(lambdaExpression))] string lambdaExpressionBody = StringConstants.NaN)
        where TValue : class, IValueProvider, new();

    /// <include file="Docs.xml" path='*/IEvaluationScope/Evaluate-switch/*'/>
    TValue Evaluate<TCase, TValue>(Func<SwitchExpression<TCase, TValue>.ResultEvaluator> getResultEvaluatorFunc, [CallerMemberName] string name = StringConstants.NaN)
         where TCase : struct, Enum
         where TValue : class, IValueProvider, new();
}