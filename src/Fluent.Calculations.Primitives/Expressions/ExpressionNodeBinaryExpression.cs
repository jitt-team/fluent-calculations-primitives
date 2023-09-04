namespace Fluent.Calculations.Primitives.Expressions;

public class ExpressionNodeBinaryExpression : ExpressionNode
{
    internal static ExpressionNode Create(string operationBody) => new ExpressionNodeBinaryExpression(operationBody);

    internal ExpressionNodeBinaryExpression(string body) : base(body, ExpressionNodeType.BinaryExpression)
    {

    }
}
