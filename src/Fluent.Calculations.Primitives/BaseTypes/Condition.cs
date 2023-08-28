namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Utils;
using System.Numerics;
using System.Runtime.CompilerServices;

public class Condition : Value,
    IEqualityOperators<Condition, Condition, Condition>,
    IBitwiseOperators<Condition, Condition, Condition>
{
    public override string ToString() => $"{Name}";

    public Condition() : this(CreateValueArgs.Compose("NaN", ExpressionNodeConstant.Create(false.ToString()), 0))
    {
    }

    public Condition(CreateValueArgs createValueArgs) : base(createValueArgs) { }

    public bool IsTrue => PrimitiveValue > 0;

    public override IValue Default => False();

    public static bool operator true(Condition condition) => condition.IsTrue;

    public static bool operator false(Condition condition) => !condition.IsTrue;


    public static implicit operator bool(Condition condition) => condition.IsTrue;

    public static Condition True([CallerMemberName] string expressionName = "") => new Condition(CreateValueArgs.Compose(expressionName, ExpressionNodeConstant.Create(true.ToString()), 1));

    public static Condition False([CallerMemberName] string expressionName = "") => new Condition(CreateValueArgs.Compose(expressionName, ExpressionNodeConstant.Create(false.ToString()), 0));

    public static Condition operator &(Condition left, Condition right) => left.And(right);

    public static Condition operator |(Condition left, Condition right) => left.Or(right);

    public static Condition operator ==(Condition? left, Condition? right) => Enforce.NotNull(left).IsEqualToRight(right);

    public static Condition operator !=(Condition? left, Condition? right) => Enforce.NotNull(left).NotEqualToRight(right);

    public static Condition operator ^(Condition left, Condition right) => left.ExlusiveOr(right);

    public static Condition operator ~(Condition value) => value.OnesComplement();

    private Condition OnesComplement() => throw new NotSupportedException();

    private Condition ExlusiveOr(Condition value) => ReturnCondition(value, (a, b) => a ^ b);

    private Condition IsEqualToRight(Condition? right) => ReturnCondition(Enforce.NotNull(right), (a, b) => a == b);

    private Condition NotEqualToRight(Condition? right) => ReturnCondition(Enforce.NotNull(right), (a, b) => a != b);

    private Condition And(Condition value) => ReturnCondition(value, (a, b) => a & b);

    private Condition Or(Condition value) => ReturnCondition(value, (a, b) => a & b);

    private Condition ReturnCondition(IValue value, Func<bool, bool, bool> compareFunc,
        [CallerMemberName] string operatorName = "") =>
        Return<Condition, bool>(value, (a, b) => compareFunc((Condition)a, (Condition)b), operatorName);

    public override IValue Compose(CreateValueArgs args) => new Condition(args);

    public override bool Equals(object? obj) => Equals(obj as IValue);

    public override int GetHashCode() => base.GetHashCode();
}