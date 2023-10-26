namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Runtime.CompilerServices;

internal static class MathExpressionHandler
{
    public static ResultType HandleWithSingleArgument<ResultType>(
        IValue input,
        Func<decimal, decimal> primitiveCalcFunc,
        [CallerMemberName] string methodName = StringConstants.NaN) where ResultType : IValue, new()
    {
        return (ResultType)MakeOfResultType();

        IValue MakeOfResultType() => new ResultType().MakeOfThisType(MakeValueArgs.Compose(methodName, MakeExpressionNode(), ToPrimitiveResult()));
        ExpressionNode MakeExpressionNode() => new ExpressionNode(MakeBinaryExpressionBody(), ExpressionNodeType.BinaryExpression).WithArguments(input);
        decimal ToPrimitiveResult() => primitiveCalcFunc(input.Primitive);
        string MakeBinaryExpressionBody() => $"{methodName}({input})";
    }

    public static ResultType HandleWithTwoArguments<ResultType>(
        IValue left,
        IValue right,
        Func<decimal, decimal, decimal> primitiveCalcFunc,
        [CallerMemberName] string methodName = StringConstants.NaN) where ResultType : IValue, new() =>
        TwoPartExpressionHandler.Handle<ResultType, decimal>(left, right, (a, b) => primitiveCalcFunc(a.Primitive, b.Primitive), methodName);
}
