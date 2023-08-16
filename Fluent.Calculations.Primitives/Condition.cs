using System.Runtime.CompilerServices;
namespace Fluent.Calculations.Primitives;

public class Condition : Value
{
    public override string ToString() => $"{Name}:{IsTrue}";

    public Condition(string name, decimal primitiveValue) : base(name, primitiveValue) { }

    public Condition(string expressionName, string expressionBody, decimal primitiveValue, ArgumentsList arguments, TagsList tags) :
        base(expressionName, expressionBody, primitiveValue, arguments, tags)
    { }

    public Condition(CreateValueArgs createValueArgs) : base(createValueArgs)
    {
    }

    public static implicit operator bool(Condition condition) => condition.IsTrue;

    public bool IsTrue => PrimitiveValue > 0;

    public static Condition True([CallerMemberName] string expressionName = "") => new Condition(expressionName, 1);

    public static Condition False([CallerMemberName] string expressionName = "") => new Condition(expressionName, 0);

    public override IValue ToExpressionResult(CreateValueArgs args) => new Condition(args);
}