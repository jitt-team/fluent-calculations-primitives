using Fluent.Calculations.Primitives.BaseTypes;

namespace Fluent.Calculations.Primitives.Expressions;

public class ExpressionNodeConditional : ExpressionNode
{
    internal ExpressionNodeConditional(string body) : base(body) { }

    public Condition Condition { get; set; } = Condition.False();

    public IValue IfTrue { get; set; } = Condition.False();

    public IValue IfFalse { get; set; } = Condition.False();

    public override ArgumentsCollection Arguments => ArgumentsCollection.CreateFrom(new[] { Condition, IfTrue, IfFalse });
}
