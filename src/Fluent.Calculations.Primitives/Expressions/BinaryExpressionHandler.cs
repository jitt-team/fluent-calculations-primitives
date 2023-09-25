namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;

internal class BinaryExpressionHandler
{
    public static ResultType Handle<ResultType, ResultPrimitiveType>(
        IValue left,
        IValue right,
        Func<IValue, IValue, ResultPrimitiveType> calcFunc,
        string operatorName) where ResultType : IValue, new()
    {
        return (ResultType)MakeInstance();

        IValue MakeInstance() => new ResultType().Make(MakeValueArgs.Compose(operatorName, MakeExpressionNode(), ToResult()));
        ExpressionNode MakeExpressionNode() => new ExpressionNode(MakeBinaryExpressionBody(), ExpressionNodeType.BinaryExpression).WithArguments(left, right);
        decimal ToResult() => Convert.ToDecimal(calcFunc(left, right));
        string MakeBinaryExpressionBody() => $"{left} {BinaryExpressionOperatorTranslator.MethodNameToOperator(operatorName)} {right}";
    }
}
