namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Utils;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

/// <summary>
/// TBD
/// </summary>
/// <remarks>TBD</remarks>
/// <param name="makeValueArgs">TBD</param>
[DebuggerDisplay("Name = {Name}, Value = {IsTrue}")]
public sealed class Condition(MakeValueArgs makeValueArgs) : Value(makeValueArgs),
    IEqualityOperators<Condition, Condition, Condition>,
    IBitwiseOperators<Condition, Condition, Condition>
{
    /// <include file="Docs.xml" path='*/Condition/ToString/*'/>
    public override string ToString() => $"{Name}";

    /// <include file="Docs.xml" path='*/Condition/PrimitiveString/*'/>
    public override string PrimitiveString => $"{IsTrue}";

    /// <include file="Docs.xml" path='*/Condition/ctor/*'/>
    public Condition() : this(MakeValueArgs.Compose(StringConstants.NaN, new ExpressionNode(false.ToString(), ExpressionNodeType.Constant), 0)) { }

    /// <include file="Docs.xml" path='*/Condition/IsTrue/*'/>
    public bool IsTrue => Primitive > 0;

    /// <include file="Docs.xml" path='*/Condition/MakeDefault/*'/>
    public override IValueProvider MakeDefault() => False();

    /// <include file="Docs.xml" path='*/Condition/implicit_Bool/*'/>
    public static implicit operator Condition(bool value) => value ? True() : False();

    /// <include file="Docs.xml" path='*/Condition/implicit_Condition/*'/>
    public static implicit operator bool(Condition condition) => condition.IsTrue;

    /// <include file="Docs.xml" path='*/Condition/op_True/*'/>
    public static bool operator true(Condition condition) => condition.IsTrue;

    /// <include file="Docs.xml" path='*/Condition/op_False/*'/>
    public static bool operator false(Condition condition) => !condition.IsTrue;

    /// <include file="Docs.xml" path='*/Condition/True/*'/>
    public static Condition True([CallerMemberName] string expressionName = "") => True(StringConstants.NaN, expressionName);

    /// <include file="Docs.xml" path='*/Condition/True-scope/*'/>
    public static Condition True(string scope, [CallerMemberName] string expressionName = "") => new(MakeValueArgs.Compose(expressionName, new ExpressionNode(true.ToString(), ExpressionNodeType.Constant), 1, ValueOriginType.Constant, scope));

    /// <include file="Docs.xml" path='*/Condition/False/*'/>
    public static Condition False([CallerMemberName] string expressionName = "") => False(StringConstants.NaN, expressionName);

    /// <include file="Docs.xml" path='*/Condition/False-scope/*'/>
    public static Condition False(string scope, [CallerMemberName] string expressionName = "") => new(MakeValueArgs.Compose(expressionName, new ExpressionNode(false.ToString(), ExpressionNodeType.Constant), 0, ValueOriginType.Constant, scope));

    /// <include file="Docs.xml" path='*/Condition/MakeOfThisType/*'/>
    public override IValueProvider MakeOfThisType(MakeValueArgs args) => new Condition(args);

    /// <include file="Docs.xml" path='*/Condition/Equals/*'/>
    public override bool Equals(object? obj) => Equals(obj as IValueProvider);

    /// <include file="Docs.xml" path='*/Condition/GetHashCode/*'/>
    public override int GetHashCode() => base.GetHashCode();

    /// <include file="Docs.xml" path='*/Condition/op_And/*'/>
    public static Condition operator &(Condition left, Condition right) => left.And(right);

    /// <include file="Docs.xml" path='*/Condition/op_Or/*'/>
    public static Condition operator |(Condition left, Condition right) => left.Or(right);

    /// <include file="Docs.xml" path='*/Condition/op_Equality/*'/>
    public static Condition operator ==(Condition? left, Condition? right) => Enforce.NotNull(left).IsEqualToRight(right);

    /// <include file="Docs.xml" path='*/Condition/op_Inequality/*'/>
    public static Condition operator !=(Condition? left, Condition? right) => Enforce.NotNull(left).NotEqualToRight(right);

    /// <include file="Docs.xml" path='*/Condition/op_ExlusiveOr/*'/>
    public static Condition operator ^(Condition left, Condition right) => left.ExlusiveOr(right);

    /// <include file="Docs.xml" path='*/Condition/op_OnesComplement/*'/>
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