namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Collections;
using System.Runtime.CompilerServices;

internal class AggregateExpressionHandler
{
    public static TSource HandleAggregate<TSource>(IValues<TSource> values, Func<IEnumerable<TSource>, Func<TSource, decimal>, decimal> aggregateFunc, [CallerMemberName] string methodName = Constants.NaN) where TSource : class, IValue, new() =>
        HandleAggregate(values, () => aggregateFunc(values, OfPrimitiveValue), methodName);

    private static ValueType HandleAggregate<ValueType>(IValues<ValueType> valueCollection, Func<decimal> primitiveValueAggregateFunc, string operatorName) where ValueType : class, IValue, new()
    {
        return (ValueType)MakeElementInstance();

        IValue MakeElementInstance() => new Values<ValueType>().MakeElement(MakeValueArgs.Compose(operatorName, MakeExpressionNode(), primitiveValueAggregateFunc()));
        ExpressionNode MakeExpressionNode() => new ExpressionNode(MakeCollectionExpressionBody(), ExpressionNodeType.Collection).WithArguments((IValue)valueCollection);
        string MakeCollectionExpressionBody() => $"{BinaryExpressionOperatorTranslator.MethodNameToOperator(operatorName)} of {valueCollection}";
    }

    private static Func<IValue, decimal> OfPrimitiveValue = new Func<IValue, decimal>(value => value.Primitive);
}
