using System.Runtime.CompilerServices;
namespace Fluent.Calculations.Primitives;

public class Condition : Value
{
    public override string ToString() => $"{Name}:{IsTrue}";

    public Condition() : base("False", 0) { }

    public Condition(string name, decimal primitiveValue) : base(name, primitiveValue) { }

    public Condition(string expressionName, ExpressionNode expressionNode, decimal primitiveValue, ArgumentsList arguments, TagsList tags) :
        base(expressionName, expressionNode, primitiveValue, arguments, tags)
    { }

    public Condition(CreateValueArgs createValueArgs) : base(createValueArgs)
    {
    }

    public static implicit operator bool(Condition condition) => condition.IsTrue;

    //TODO refactor 1/0 acrobatics
    public static explicit operator Condition(bool condition) => new Condition("tbd", condition ? 1 : 0);

    public bool IsTrue => PrimitiveValue > 0;

    public static Condition True([CallerMemberName] string expressionName = "") => new Condition(expressionName, 1);

    public static Condition False([CallerMemberName] string expressionName = "") => new Condition(expressionName, 0);

    public override IValue ToExpressionResult(CreateValueArgs args) => new Condition(args);
}