namespace Fluent.Calculations.Primitives
{
    public class ExpressionNode
    {
        internal ExpressionNode(string body)
        {
            Body = Body;
        }

        internal static ExpressionNode Default => new ExpressionNode("");

        public string Body { get; internal set; } = "";

        internal ExpressionNode WithBody(string body)
        {
            Body = body;
            return this;
        }
    }
}
