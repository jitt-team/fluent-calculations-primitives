namespace Fluent.Calculations.Primitives.Expressions;

public class ExpressionNodeConditional3 : ExpressionNode
{
    internal ExpressionNodeConditional3(string body) : base(body) { }

    public Condition Condition { get; set; } = Condition.False();

    public IValue IfTrue { get; set; }

    public IValue IfFalse { get; set; }

    public override ArgumentsList Arguments => ArgumentsList.CreateFrom(new[] { Condition, IfTrue, IfFalse });
}
