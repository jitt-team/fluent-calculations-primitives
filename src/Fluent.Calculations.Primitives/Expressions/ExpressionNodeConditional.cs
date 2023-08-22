namespace Fluent.Calculations.Primitives.Expressions;

public class ExpressionNodeConditional : ExpressionNode
{
    internal ExpressionNodeConditional(string body) : base(body) { }

    public Condition Condition { get; set; } = Condition.False();

    public IValue IfTrue { get; set; }

    public IValue IfFalse { get; set; }

    public override ArgumentsList Arguments => ArgumentsList.CreateFrom(new[] { Condition, IfTrue, IfFalse });
}
