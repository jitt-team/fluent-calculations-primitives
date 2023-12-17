namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;

internal static class BinaryOperatorHandler
{
    public static ResultType Handle<ResultType, ResultPrimitiveType>(
         IValueProvider left,
         IValueProvider right,
         Func<IValueProvider, IValueProvider, ResultPrimitiveType> primitiveCalcFunc,
         string operatorName,
         string expressionType) where ResultType : IValueProvider, new() =>
        Handle<ResultType, ResultPrimitiveType>(left, right, primitiveCalcFunc, operatorName, expressionType, ComposeBinaryExpressionBody);

    public static ResultType Handle<ResultType, ResultPrimitiveType>(
        IValueProvider left,
        IValueProvider right,
        Func<IValueProvider, IValueProvider, ResultPrimitiveType> primitiveCalcFunc,
        string operatorName,
        string expressionType,
        Func<IValueProvider, IValueProvider, string, string> composeBodyFunc) where ResultType : IValueProvider, new()
    {
        return (ResultType)MakeOfResultType();

        IValueProvider MakeOfResultType() => new ResultType().MakeOfThisType(MakeValueArgs.Compose(operatorName, MakeExpressionNode(), ToPrimitiveResult(), ValueOriginType.Operation));
        ExpressionNode MakeExpressionNode() => new ExpressionNode(composeBodyFunc(left, right, operatorName), expressionType).WithArguments(left, right);
        decimal ToPrimitiveResult() => Convert.ToDecimal(primitiveCalcFunc(left, right));
    }

    private static string ComposeBinaryExpressionBody(IValueProvider left, IValueProvider right, string operatorName) => $"{left} {LanguageOperatorTranslator.MethodNameToOperator(operatorName)} {right}";
}
