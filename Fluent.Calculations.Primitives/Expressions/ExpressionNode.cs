namespace Fluent.Calculations.Primitives
{
    public class ExpressionNode
    {
        public override string ToString() => Body;

        internal ExpressionNode(string body)
        {
            Body = body;
        }

        internal static ExpressionNode Default => new ExpressionNode("");

        public string Body { get; internal set; } = "";

        internal ExpressionNode WithBody(string body)
        {
            Body = body;
            return this;
        }

        public virtual ArgumentsList Arguments { get; internal set; } = ArgumentsList.Empty;

        public ExpressionNode WithArguments(IValue[] arguments)
        {
            Arguments = new ArgumentsList(arguments);
            return this;
        }

        public ExpressionNode WithArguments(IValue a, params IValue[] b) => WithArguments(new[] { a }.Concat(b).ToArray());

    }
}
