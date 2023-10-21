namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Collections;
using System.Runtime.CompilerServices;

internal class CollectionExpressionHandler
{
    public static TSource Handle<TSource>(IValues<TSource> source, Func<IEnumerable<TSource>, Func<TSource, decimal>, decimal> aggregateFunc, [CallerMemberName] string methodName = Constants.NaN) where TSource : class, IValue, new() =>
        HandleAggregate(source, () => aggregateFunc(source, SelectPrimitiveValue), methodName);

    private static TSource HandleAggregate<TSource>(IValues<TSource> source, Func<decimal> primitiveValueAggregateFunc, string operatorName) where TSource : class, IValue, new()
    {
        return (TSource)MakeResultInstance();

        IValue MakeResultInstance() => new Values<TSource>().MakeElement(MakeValueArgs.Compose(operatorName, MakeExpressionNode(), primitiveValueAggregateFunc()));
        ExpressionNode MakeExpressionNode() => new ExpressionNode(MakeCollectionExpressionBody(), ExpressionNodeType.Collection).WithArguments((IValue)source);
        string MakeCollectionExpressionBody() => $"{BinaryExpressionOperatorTranslator.MethodNameToOperator(operatorName)} of {source}";
    }

    private static Func<IValue, decimal> SelectPrimitiveValue = new Func<IValue, decimal>(value => value.Primitive);
}
