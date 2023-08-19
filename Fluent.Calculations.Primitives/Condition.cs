using System.Runtime.CompilerServices;
namespace Fluent.Calculations.Primitives;

public class Condition : Value
{
    public override string ToString() => $"{Name}:{IsTrue}";

    public Condition() : base("False", 0)
    {

    }

    public Condition(string expressionName, decimal primitiveValue) : base(expressionName, primitiveValue)
    {
        Expresion = new ExpressionNodeConstant(Convert.ToBoolean(primitiveValue).ToString());
    }

    public Condition(string expressionName, ExpressionNode expressionNode, decimal primitiveValue, ArgumentsList arguments, TagsList tags) :
        base(expressionName, expressionNode, primitiveValue, arguments, tags)
    { }

    public Condition(CreateValueArgs createValueArgs) : base(createValueArgs)
    {
    }

    public static bool operator true(Condition condition) => condition.IsTrue;

    public static bool operator false(Condition condition) => !condition.IsTrue;

    public static Condition operator &(Condition left, Condition right) => left.And(right);

    public static Condition operator |(Condition left, Condition right) => left.Or(right);

    public static implicit operator bool(Condition condition) => condition.IsTrue;

    //TODO refactor 1/0 acrobatics
    public static explicit operator Condition(bool condition) => new Condition("tbd", condition ? 1 : 0);

    public bool IsTrue => PrimitiveValue > 0;

    public static Condition True([CallerMemberName] string expressionName = "") => new Condition(expressionName, 1);

    public static Condition False([CallerMemberName] string expressionName = "") => new Condition(expressionName, 0);

    private Condition And(Condition value) => this.ReturnCondition(value, (a, b) => a & b);

    private Condition Or(Condition value) => this.ReturnCondition(value, (a, b) => a & b);

    private Condition ReturnCondition(IValue value, Func<bool, bool, bool> compareFunc,
        [CallerMemberName] string operatorName = "") =>
        Return<Condition, bool>(value, (a, b) => compareFunc((Condition)a, (Condition)b), operatorName);

    public override IValue ToExpressionResult(CreateValueArgs args) => new Condition(args);
}