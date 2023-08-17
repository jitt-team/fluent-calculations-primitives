namespace Fluent.Calculations.Primitives
{
    public class ExpressionNodeMath : ExpressionNode
    {
        internal static ExpressionNode Create(string operationBody) => new ExpressionNodeMath(operationBody);

        internal ExpressionNodeMath(string body) : base(body)
        {

        }
    }
}
