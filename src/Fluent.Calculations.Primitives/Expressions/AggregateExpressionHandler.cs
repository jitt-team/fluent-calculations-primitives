namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Collections;

internal class AggregateExpressionHandler
{
    public static CollectionElementType HandleAggregate<CollectionElementType>(
        IValues<CollectionElementType> valueCollection,
        Func<IEnumerable<CollectionElementType>, decimal> aggregateCalcFunc,
        string operatorName)
        where CollectionElementType : class, IValue, new()
    {

        return (CollectionElementType)MakeElementInstance();

        IValue MakeElementInstance() => new Values<CollectionElementType>().MakeElement(MakeValueArgs.Compose(operatorName, MakeExpressionNode(), ToPrimitiveResult()));
        ExpressionNode MakeExpressionNode() => new ExpressionNode(MakeCollectionExpressionBody(), ExpressionNodeType.Collection).WithArguments((IValue)valueCollection);
        decimal ToPrimitiveResult() => Convert.ToDecimal(aggregateCalcFunc(valueCollection));
        string MakeCollectionExpressionBody() => $"{BinaryExpressionOperatorTranslator.MethodNameToOperator(operatorName)} of {valueCollection}";
    }
}
