namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Collections;
using System.Runtime.CompilerServices;

internal static class CollectionExpressionHandler
{
    public static TSource Handle<TSource>(this IValuesProvider<TSource> source, Func<IEnumerable<TSource>, Func<TSource, decimal>, decimal> aggregateFunc, [CallerMemberName] string methodName = StringConstants.NaN) where TSource : class, IValueProvider, new() =>
        HandleInternal(source, () => aggregateFunc(source, SelectPrimitiveValue), methodName);

    private static TSource HandleInternal<TSource>(IValuesProvider<TSource> source, Func<decimal> primitiveValueAggregateFunc, string operatorName) where TSource : class, IValueProvider, new()
    {
        return (TSource)MakeOfSourceType();

        IValueProvider MakeOfSourceType() => new Values<TSource>().MakeOfThisElementType(MakeValueArgs.Compose(operatorName, MakeExpressionNode(), primitiveValueAggregateFunc()));
        ExpressionNode MakeExpressionNode() => new ExpressionNode(MakeCollectionExpressionBody(), ExpressionNodeType.Collection).WithArguments((IValueProvider)source);
        string MakeCollectionExpressionBody() => $"{operatorName}({source})";
    }

    private static Func<IValueProvider, decimal> SelectPrimitiveValue = new Func<IValueProvider, decimal>(value => value.Primitive);
}
