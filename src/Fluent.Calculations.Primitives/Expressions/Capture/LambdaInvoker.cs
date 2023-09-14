using Fluent.Calculations.Primitives.BaseTypes;
using System.Linq.Expressions;

namespace Fluent.Calculations.Primitives.Expressions.Capture
{
    internal class LambdaInvoker
    {
        public static IValue DynamicInvoke(Expression expression) => (IValue)EnsureNotNull(Expression.Lambda(expression).Compile().DynamicInvoke(), expression);

        private static object EnsureNotNull(object? obj, Expression body) => obj ?? throw new InvalidOperationException(@$"Expression ""{body}"" resulted in Null");

    }
}
