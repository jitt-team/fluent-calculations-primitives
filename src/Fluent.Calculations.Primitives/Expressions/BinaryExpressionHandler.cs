using Fluent.Calculations.Primitives.BaseTypes;

namespace Fluent.Calculations.Primitives.Expressions
{
    internal class BinaryExpressionHandler
    {
        public static ResultType Handle<ResultType, ResultPrimitiveType>(
            IValue left,
            IValue right,
            Func<IValue, IValue, ResultPrimitiveType> calcFunc,
            string operatorName) where ResultType : IValue, new()
        {
            ExpressionNode expressionNode = new ExpressionNode(MakeBinaryExpressionBody(), ExpressionNodeType.BinaryExpression)
                    .WithArguments(left, right);

            IValue result = new ResultType().Make(MakeValueArgs.Compose(operatorName, expressionNode, ToResult()));

            return (ResultType)result;

            decimal ToResult() => Convert.ToDecimal(calcFunc(left, right));
            string MakeBinaryExpressionBody() => $"{left} {BinaryExpressionLanguageConverter.MethodNameToOperator(operatorName)} {right}";
        }
    }
}
