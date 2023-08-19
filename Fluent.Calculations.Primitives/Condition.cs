using Fluent.Calculations.Primitives.Expressions;
using System.Runtime.CompilerServices;
namespace Fluent.Calculations.Primitives;

public class Condition : Value
{
    public override string ToString() => $"{Name}";

    public Condition() : this(CreateValueArgs.Compose("Default", ExpressionNodeConstant.Create(false.ToString()), 0))
    {
    }

    public Condition(CreateValueArgs createValueArgs) : base(createValueArgs) { }

    public static Condition True([CallerMemberName] string expressionName = "") => new Condition(CreateValueArgs.Compose(expressionName, ExpressionNodeConstant.Create(true.ToString()), 1));

    public static Condition False([CallerMemberName] string expressionName = "") => new Condition(CreateValueArgs.Compose(expressionName, ExpressionNodeConstant.Create(false.ToString()), 0));

    public static bool operator true(Condition condition) => condition.IsTrue;

    public static bool operator false(Condition condition) => !condition.IsTrue;

    public static Condition operator &(Condition left, Condition right) => left.And(right);

    public static Condition operator |(Condition left, Condition right) => left.Or(right);

    public static implicit operator bool(Condition condition) => condition.IsTrue;

    public bool IsTrue => PrimitiveValue > 0;

    private Condition And(Condition value) => this.ReturnCondition(value, (a, b) => a & b);

    private Condition Or(Condition value) => this.ReturnCondition(value, (a, b) => a & b);

    private Condition ReturnCondition(IValue value, Func<bool, bool, bool> compareFunc,
        [CallerMemberName] string operatorName = "") =>
        Return<Condition, bool>(value, (a, b) => compareFunc((Condition)a, (Condition)b), operatorName);

    public override IValue ToExpressionResult(CreateValueArgs args) => new Condition(args);
}