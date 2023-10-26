namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;

internal static class TwoPartExpressionHandler
{
    public static ResultType Handle<ResultType, ResultPrimitiveType>(
        IValue left,
        IValue right,
        Func<IValue, IValue, ResultPrimitiveType> primitiveCalcFunc,
        string operatorName) where ResultType : IValue, new()
    {
        return (ResultType)MakeOfResultType();

        IValue MakeOfResultType() => new ResultType().MakeOfThisType(MakeValueArgs.Compose(operatorName, MakeExpressionNode(), ToPrimitiveResult()));
        ExpressionNode MakeExpressionNode() => new ExpressionNode(MakeBinaryExpressionBody(), ExpressionNodeType.BinaryExpression).WithArguments(left, right);
        decimal ToPrimitiveResult() => Convert.ToDecimal(primitiveCalcFunc(left, right));
        string MakeBinaryExpressionBody() => $"{left} {LanguageOperatorTranslator.MethodNameToOperator(operatorName)} {right}";
    }
}
