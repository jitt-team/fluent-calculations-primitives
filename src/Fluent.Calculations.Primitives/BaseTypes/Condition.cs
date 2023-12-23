namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Utils;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

/// <include file="Docs/IntelliSense.xml" path='docs/members[@name="Condition"]/class/*' />
[DebuggerDisplay("Name = {Name}, Value = {IsTrue}")]
public sealed class Condition : Value,
    IEqualityOperators<Condition, Condition, Condition>,
    IBitwiseOperators<Condition, Condition, Condition>
{
    public override string ToString() => $"{Name}";

    public override string PrimitiveString => $"{IsTrue}";

    public Condition(MakeValueArgs makeValueArgs) : base(makeValueArgs) { }

    public Condition() : this(MakeValueArgs.Compose(StringConstants.NaN, new ExpressionNode(false.ToString(), ExpressionNodeType.Constant), 0))
    {
    }

    public bool IsTrue => Primitive > 0;

    public override IValueProvider MakeDefault() => False();


    public static  implicit operator Condition(bool condition) => condition ? True() : False();

    public static bool operator true(Condition condition) => condition.IsTrue;

    public static bool operator false(Condition condition) => !condition.IsTrue;

    public static implicit operator bool(Condition condition) => condition.IsTrue;

    public static Condition True([CallerMemberName] string expressionName = "") => True(StringConstants.NaN, expressionName);

    public static Condition True(string scope, [CallerMemberName] string expressionName = "") => new(MakeValueArgs.Compose(expressionName, new ExpressionNode(true.ToString(), ExpressionNodeType.Constant), 1, ValueOriginType.Constant, scope));

    public static Condition False([CallerMemberName] string expressionName = "") => False(StringConstants.NaN, expressionName);

    public static Condition False(string scope, [CallerMemberName] string expressionName = "") => new(MakeValueArgs.Compose(expressionName, new ExpressionNode(false.ToString(), ExpressionNodeType.Constant), 0, ValueOriginType.Constant, scope));
    
    public override IValueProvider MakeOfThisType(MakeValueArgs args) => new Condition(args);

    public override bool Equals(object? obj) => Equals(obj as IValueProvider);

    public override int GetHashCode() => base.GetHashCode();

    public static Condition operator &(Condition left, Condition right) => left.And(right);

    public static Condition operator |(Condition left, Condition right) => left.Or(right);

    public static Condition operator ==(Condition? left, Condition? right) => Enforce.NotNull(left).IsEqualToRight(right);

    public static Condition operator !=(Condition? left, Condition? right) => Enforce.NotNull(left).NotEqualToRight(right);

    public static Condition operator ^(Condition left, Condition right) => left.ExlusiveOr(right);

    public static Condition operator ~(Condition value) => value.OnesComplement();

    private Condition OnesComplement() => throw new NotSupportedException();

    private Condition ExlusiveOr(Condition value) => HandleBinaryOperation(value, (a, b) => a ^ b);

    private Condition IsEqualToRight(Condition? right) => HandleBinaryOperation(Enforce.NotNull(right), (a, b) => a == b);

    private Condition NotEqualToRight(Condition? right) => HandleBinaryOperation(Enforce.NotNull(right), (a, b) => a != b);

    private Condition And(Condition value) => HandleBinaryOperation(value, (a, b) => a & b);

    private Condition Or(Condition value) => HandleBinaryOperation(value, (a, b) => a & b);

    private Condition HandleBinaryOperation(IValueProvider value, Func<bool, bool, bool> compareFunc,
            [CallerMemberName] string operatorName = StringConstants.NaN) =>
            HandleBinaryOperation<Condition, bool>(value, (a, b) => compareFunc((Condition)a, (Condition)b), operatorName);
}