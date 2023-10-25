namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Collections;
using System.Runtime.CompilerServices;

internal static class CollectionExpressionHandler
{
    public static TSource Handle<TSource>(this IValues<TSource> source, Func<IEnumerable<TSource>, Func<TSource, decimal>, decimal> aggregateFunc, [CallerMemberName] string methodName = StringConstants.NaN) where TSource : class, IValue, new() =>
        HandleInternal(source, () => aggregateFunc(source, SelectPrimitiveValue), methodName);

    private static TSource HandleInternal<TSource>(IValues<TSource> source, Func<decimal> primitiveValueAggregateFunc, string operatorName) where TSource : class, IValue, new()
    {
        return (TSource)MakeOfSourceType();

        IValue MakeOfSourceType() => new Values<TSource>().MakeOfThisElementType(MakeValueArgs.Compose(operatorName, MakeExpressionNode(), primitiveValueAggregateFunc()));
        ExpressionNode MakeExpressionNode() => new ExpressionNode(MakeCollectionExpressionBody(), ExpressionNodeType.Collection).WithArguments((IValue)source);
        string MakeCollectionExpressionBody() => $"{operatorName}({source})";
    }

    private static Func<IValue, decimal> SelectPrimitiveValue = new Func<IValue, decimal>(value => value.Primitive);
}
