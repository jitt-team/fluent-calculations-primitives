namespace Fluent.Calculations.Primitives
{
    public class ExpressionNode
    {
        internal static ExpressionNode Default => new ExpressionNode();

        public string Body { get; internal set; } = "";

        internal ExpressionNode WithBody(string body)
        {
            Body = body;
            return this;
        }

        public virtual ArgumentsList Arguments { get; private set; } = ArgumentsList.Empty;
    }

    public class ExpressionNodeConditional : ExpressionNode
    {
        public override string ToString()
        {
            return $"{Condition} ? {IfTrue} : {IfFalse}";
        }

        public Condition Condition { get; set; }

        public IValue IfTrue { get; set; }

        public IValue IfFalse { get; set; }

        public override ArgumentsList Arguments => ArgumentsList.CreateFrom(new[] { Condition, IfTrue, IfFalse });
    }
}
