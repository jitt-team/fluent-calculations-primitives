namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;

internal static class BinaryOperatorHandler
{
    public static ResultType Handle<ResultType, ResultPrimitiveType>(
         IValue left,
         IValue right,
         Func<IValue, IValue, ResultPrimitiveType> primitiveCalcFunc,
         string operatorName,
         string expressionType) where ResultType : IValue, new() =>
        Handle<ResultType, ResultPrimitiveType>(left, right, primitiveCalcFunc, operatorName, expressionType, ComposeBinaryExpressionBody);

    public static ResultType Handle<ResultType, ResultPrimitiveType>(
        IValue left,
        IValue right,
        Func<IValue, IValue, ResultPrimitiveType> primitiveCalcFunc,
        string operatorName,
        string expressionType,
        Func<IValue, IValue, string, string> composeBodyFunc) where ResultType : IValue, new()
    {
        return (ResultType)MakeOfResultType();

        IValue MakeOfResultType() => new ResultType().MakeOfThisType(MakeValueArgs.Compose(operatorName, MakeExpressionNode(), ToPrimitiveResult()));
        ExpressionNode MakeExpressionNode() => new ExpressionNode(composeBodyFunc(left, right, operatorName), expressionType).WithArguments(left, right);
        decimal ToPrimitiveResult() => Convert.ToDecimal(primitiveCalcFunc(left, right));
    }

    private static string ComposeBinaryExpressionBody(IValue left, IValue right, string operatorName) => $"{left} {LanguageOperatorTranslator.MethodNameToOperator(operatorName)} {right}";
}
