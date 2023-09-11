namespace Fluent.Calculations.Primitives.Expressions.Capture;
using System.Linq.Expressions;

internal class ExpressionMemberExtractor : IExpressionMemberExtractor
{
    public Expression[] ExtractBinaryExpressionMembers(BinaryExpression expression) =>
        new[] { expression.Left, expression.Right };

    public Expression[] ExtractConditionalExpressionMembers(ConditionalExpression expression) =>
        new[] { expression.Test, expression.IfTrue, expression.IfFalse };

    public bool IsBinaryExpression(ExpressionType expressionType) => BinaryExpressionTypes.Contains(expressionType);

    private static ExpressionType[] BinaryExpressionTypes = new[] {
        ExpressionType.Add,
        ExpressionType.Subtract,
        ExpressionType.Multiply,
        ExpressionType.Divide,
        ExpressionType.GreaterThan,
        ExpressionType.LessThan,
        ExpressionType.GreaterThanOrEqual,
        ExpressionType.LessThanOrEqual,
        ExpressionType.Modulo,
        ExpressionType.Power,
        ExpressionType.Equal,
        ExpressionType.NotEqual,
        ExpressionType.Add,
        ExpressionType.AndAlso,
        ExpressionType.OrElse,
        ExpressionType.Or,
    };
}
