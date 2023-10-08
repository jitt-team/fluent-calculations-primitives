using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Fluent.Calculations.Primitives
{
    public interface IEvaluationContext<TResult> where TResult : class, IValue, new()
    {
        ExpressionResultValue Evaluate<ExpressionResultValue>(Expression<Func<ExpressionResultValue>> lambdaExpression, [CallerMemberName] string name = "NaN", [CallerArgumentExpression("lambdaExpression")] string lambdaExpressionBody = "NaN") where ExpressionResultValue : class, IValue, new();
        TResult Return();
        TResult ToResult();
    }
}