namespace Fluent.Calculations.Primitives
{
    public class ExpressionNodeConditional : ExpressionNode
    {
        internal ExpressionNodeConditional(string body) : base(body)
        {

        }

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
