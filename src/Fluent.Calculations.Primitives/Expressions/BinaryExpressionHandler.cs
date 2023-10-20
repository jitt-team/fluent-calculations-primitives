namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Collections;

internal class BinaryExpressionHandler
{
    public static ResultType Handle<ResultType, ResultPrimitiveType>(
        IValue left,
        IValue right,
        Func<IValue, IValue, ResultPrimitiveType> primitiveCalcFunc,
        string operatorName) where ResultType : IValue, new()
    {
        return (ResultType)MakeInstance();

        IValue MakeInstance() => new ResultType().Make(MakeValueArgs.Compose(operatorName, MakeExpressionNode(), ToPrimitiveResult()));
        ExpressionNode MakeExpressionNode() => new ExpressionNode(MakeBinaryExpressionBody(), ExpressionNodeType.BinaryExpression).WithArguments(left, right);
        decimal ToPrimitiveResult() => Convert.ToDecimal(primitiveCalcFunc(left, right));
        string MakeBinaryExpressionBody() => $"{left} {BinaryExpressionOperatorTranslator.MethodNameToOperator(operatorName)} {right}";
    }

    public static CollectionElementType HandleAggregate<CollectionType, CollectionElementType>(
        CollectionType valueCollection,
        Func<IEnumerable<CollectionElementType>, decimal> aggregateCalcFunc,
        string operatorName)
        where CollectionElementType : class, IValue, new()
        where CollectionType : class, IEnumerable<CollectionElementType>, IValues<CollectionElementType>, new()
    {
        return (CollectionElementType)MakeElementInstance();

        IValue MakeElementInstance() => new CollectionType().MakeElement(MakeValueArgs.Compose(operatorName, MakeExpressionNode(), ToPrimitiveResult()));
        ExpressionNode MakeExpressionNode() => new ExpressionNode(MakeCollectionExpressionBody(), ExpressionNodeType.Collection).WithArguments((IValue)valueCollection);
        decimal ToPrimitiveResult() => Convert.ToDecimal(aggregateCalcFunc(valueCollection));
        string MakeCollectionExpressionBody() => $"{BinaryExpressionOperatorTranslator.MethodNameToOperator(operatorName)} of {valueCollection}";
    }
}
