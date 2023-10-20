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

    public static ResultType Handle<ResultType, ResultPrimitiveType>(
        Values<IValue> collecionValue,
        Func<IEnumerable<IValue>, ResultPrimitiveType> aggregateCalcFunc,
    string operatorName) where ResultType : IValue, new()
    {
        return (ResultType)MakeInstance();

        IValue MakeInstance() => new ResultType().Make(MakeValueArgs.Compose(operatorName, MakeExpressionNode(), ToPrimitiveResult()));
        ExpressionNode MakeExpressionNode() => new ExpressionNode(MakeCollectionExpressionBody(), ExpressionNodeType.BinaryExpression).WithArguments((IValue)collecionValue);
        decimal ToPrimitiveResult() => Convert.ToDecimal(aggregateCalcFunc(collecionValue));
        string MakeCollectionExpressionBody() => $"{BinaryExpressionOperatorTranslator.MethodNameToOperator(operatorName)} of {collecionValue}";
    }
}
