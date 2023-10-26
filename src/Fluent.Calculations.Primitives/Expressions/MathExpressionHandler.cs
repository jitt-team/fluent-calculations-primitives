namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;

internal static class MathExpressionHandler
{
    public static ResultType HandleWithSingleArgument<ResultType>(
        IValue input,
        Func<decimal, decimal> primitiveCalcFunc,
        [CallerMemberName] string methodName = StringConstants.NaN) where ResultType : IValue, new()
    {
        return (ResultType)MakeOfResultType();

        IValue MakeOfResultType() => new ResultType().MakeOfThisType(MakeValueArgs.Compose(methodName, MakeExpressionNode(), ToPrimitiveResult()));
        ExpressionNode MakeExpressionNode() => new ExpressionNode(MakeBinaryExpressionBody(), ExpressionNodeType.MathExpression).WithArguments(input);
        decimal ToPrimitiveResult() => primitiveCalcFunc(input.Primitive);
        string MakeBinaryExpressionBody() => $"{methodName}({input})";
    }

    public static ResultType HandleWithTwoArgumentsInt<ResultType>(
        IValue left,
        IValue right,
        Func<decimal, int, decimal> primitiveCalcFunc,
        [CallerMemberName] string methodName = StringConstants.NaN) where ResultType : IValue, new() =>
        BinaryOperatorHandler.Handle<ResultType, decimal>(left, right,
            (a, b) => primitiveCalcFunc(a.Primitive, Convert.ToInt32(b.Primitive)), methodName, ExpressionNodeType.MathExpression, ComposeMathMethodBody);

    public static ResultType HandleWithTwoArguments<ResultType>(
        IValue left,
        IValue right,
        Func<decimal, decimal, decimal> primitiveCalcFunc,
        [CallerMemberName] string methodName = StringConstants.NaN) where ResultType : IValue, new() =>
        BinaryOperatorHandler.Handle<ResultType, decimal>(left, right,
            (a, b) => primitiveCalcFunc(a.Primitive, b.Primitive), methodName, ExpressionNodeType.MathExpression, ComposeMathMethodBody);

    private static string ComposeMathMethodBody(IValue left, IValue right, string methodName) => $"{methodName}({left},{right})";
}
