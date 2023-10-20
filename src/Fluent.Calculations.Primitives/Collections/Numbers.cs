namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;

public class Numbers : Values<Number>
{
    public Numbers() : this(MakeValueArgs.Compose("Zero", new ExpressionNode($"0", ExpressionNodeType.Collection), 0)) { }

    public Numbers(MakeValueArgs createValueArgs) : base(createValueArgs) { }

    public override decimal Primitive { get; init; }

    public override IValue Default => new Numbers { Primitive = decimal.Zero };

    public override IValue Make(MakeValueArgs args) => new Numbers(args);

    public override IValue MakeElement(MakeValueArgs args) => new Number(args);

    public Number Sum() => this.Sum<Numbers, Number>();
}

