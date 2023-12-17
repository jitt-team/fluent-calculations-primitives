namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Runtime.CompilerServices;

internal static class MathExpressionHandler
{
    public static ResultType HandleWithSingleArgument<ResultType>(
        IValueProvider input,
        Func<decimal, decimal> primitiveCalcFunc,
        [CallerMemberName] string methodName = StringConstants.NaN) where ResultType : IValueProvider, new()
    {
        return (ResultType)MakeOfResultType();

        IValueProvider MakeOfResultType() => new ResultType().MakeOfThisType(MakeValueArgs.Compose(methodName, MakeExpressionNode(), ToPrimitiveResult(), ValueOriginType.Operation));
        ExpressionNode MakeExpressionNode() => new ExpressionNode(MakeBinaryExpressionBody(), ExpressionNodeType.MathExpression).WithArguments(input);
        decimal ToPrimitiveResult() => primitiveCalcFunc(input.Primitive);
        string MakeBinaryExpressionBody() => $"{methodName}({input})";
    }

    public static ResultType HandleWithTwoArgumentsInt<ResultType>(
        IValueProvider left,
        IValueProvider right,
        Func<decimal, int, decimal> primitiveCalcFunc,
        [CallerMemberName] string methodName = StringConstants.NaN) where ResultType : IValueProvider, new() =>
        BinaryOperatorHandler.Handle<ResultType, decimal>(left, right,
            (a, b) => primitiveCalcFunc(a.Primitive, Convert.ToInt32(b.Primitive)), methodName, ExpressionNodeType.MathExpression, ComposeMathMethodBody);

    public static ResultType HandleWithTwoArguments<ResultType>(
        IValueProvider left,
        IValueProvider right,
        Func<decimal, decimal, decimal> primitiveCalcFunc,
        [CallerMemberName] string methodName = StringConstants.NaN) where ResultType : IValueProvider, new() =>
        BinaryOperatorHandler.Handle<ResultType, decimal>(left, right,
            (a, b) => primitiveCalcFunc(a.Primitive, b.Primitive), methodName, ExpressionNodeType.MathExpression, ComposeMathMethodBody);

    private static string ComposeMathMethodBody(IValueProvider left, IValueProvider right, string methodName) => $"{methodName}({left},{right})";
}
