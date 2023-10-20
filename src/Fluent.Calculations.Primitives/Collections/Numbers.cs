namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System;
using System.Runtime.CompilerServices;

public class Numbers : Values<Number>
{
    public Numbers() : this(MakeValueArgs.Compose("Zero", new ExpressionNode($"0", ExpressionNodeType.Collection), 0)) { }

    public Numbers(MakeValueArgs createValueArgs) : base(createValueArgs) { }

    public override decimal Primitive { get; init; }

    public override IValue Default => new Numbers { Primitive = decimal.Zero };

    internal static Numbers Of(Func<Number[]> valuesFunc, [CallerArgumentExpression("valuesFunc")] string lambdaExpressionBody = Constants.NaN)
    {
        var numbers = new Numbers();
        valuesFunc().ToList().ForEach(numbers.Add);
        return numbers;
    }

    public override IValue Make(MakeValueArgs args) => new Numbers(args);

    public override IValue MakeElement(MakeValueArgs args) => new Number(args);

    public Number Sum() => this.Sum<Numbers, Number>();
}

