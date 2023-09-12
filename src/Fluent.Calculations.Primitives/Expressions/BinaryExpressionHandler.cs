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
            ExpressionNode expressionNode = new ExpressionNode(ComposeBinaryExpressionBody(), ExpressionNodeType.BinaryExpression)
                    .WithArguments(left, right);

            IValue result = new ResultType().Create(CreateValueArgs.Build(operatorName, expressionNode, ToResult()));

            return (ResultType)result;

            decimal ToResult() => Convert.ToDecimal(calcFunc(left, right));
            string ComposeBinaryExpressionBody() => $"{left} {BinaryExpressionLanguageConverter.MethodNameToOperator(operatorName)} {right}";
        }
    }
}
