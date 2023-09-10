namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Utils;
using System.Numerics;
using System.Runtime.CompilerServices;

public sealed class Condition : Value,
    IEqualityOperators<Condition, Condition, Condition>,
    IBitwiseOperators<Condition, Condition, Condition>
{
    public override string ToString() => $"{Name}";

    public override string ValueToString() => $"{IsTrue}";

    public Condition() : this(CreateValueArgs.Build("NaN", new ExpressionNode(false.ToString(), ExpressionNodeType.Constant), 0))
    {
    }

    public Condition(CreateValueArgs createValueArgs) : base(createValueArgs) { }

    public bool IsTrue => Primitive > 0;

    public override IValue Default => False();

    public static bool operator true(Condition condition) => condition.IsTrue;

    public static bool operator false(Condition condition) => !condition.IsTrue;

    public static implicit operator bool(Condition condition) => condition.IsTrue;

    public static Condition True([CallerMemberName] string expressionName = "") => new Condition(CreateValueArgs.Build(expressionName, new ExpressionNode(true.ToString(), ExpressionNodeType.Constant), 1));

    public static Condition False([CallerMemberName] string expressionName = "") => new Condition(CreateValueArgs.Build(expressionName, new ExpressionNode(false.ToString(), ExpressionNodeType.Constant), 0));

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
        HandleBinaryExpression<Condition, bool>(value, (a, b) => compareFunc((Condition)a, (Condition)b), operatorName);

    public override IValue Create(CreateValueArgs args) => new Condition(args);

    public override bool Equals(object? obj) => Equals(obj as IValue);

    public override int GetHashCode() => base.GetHashCode();
}