namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

public interface IEvaluationContext<TResult> where TResult : class, IValue, new()
{
    ExpressionResultValue Evaluate<ExpressionResultValue>(
        Expression<Func<ExpressionResultValue>> lambdaExpression,
        [CallerMemberName] string name = StringConstants.NaN,
        [CallerArgumentExpression("lambdaExpression")] string lambdaExpressionBody = StringConstants.NaN) 
        where ExpressionResultValue : class, IValue, new();

    TResult ToResult();
}