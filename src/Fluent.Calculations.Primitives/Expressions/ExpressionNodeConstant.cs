namespace Fluent.Calculations.Primitives.Expressions;

public class ExpressionNodeConstant : ExpressionNode
{
    internal ExpressionNodeConstant(string body) : base(body)
    {

    }

    internal static ExpressionNode Create(string operationBody) => new ExpressionNodeConstant(operationBody);

}
