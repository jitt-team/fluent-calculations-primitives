namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;

internal class ValueExpressionInvoker
{
    public static IValue DynamicInvoke(Expression expression) => (IValue)EnsureNotNull(Expression.Lambda(expression).Compile().DynamicInvoke(), expression);

    private static object EnsureNotNull(object? obj, Expression body) => obj ?? throw new NullExpressionResultException(body.ToString());
}
