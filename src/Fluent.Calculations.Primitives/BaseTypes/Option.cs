namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Utils;
using System.Numerics;
using System.Runtime.CompilerServices;

public static class Option
{
    public static Option<TEnum> Of<TEnum>(TEnum primitiveValue, [CallerMemberName] string fieldName = StringConstants.NaN) where TEnum : struct, Enum
        => new Option<TEnum>(MakeValueArgs.Compose(fieldName, new ExpressionNode($"{primitiveValue}", ExpressionNodeType.Constant),
            Convert.ToDecimal((int)(object)primitiveValue)) /* explore if this cast can be optimized */);
}

public class Option<T> : Value, 
    IEqualityOperators<Option<T>, Option<T>, Condition>
    where T : struct, Enum
{

    public Option() : this(MakeValueArgs.Compose(StringConstants.Zero, new ExpressionNode(StringConstants.ZeroDigit, ExpressionNodeType.Constant), 0)) { }

    public Option(Option<T> enumValue) : base(enumValue) { }

    public Option(MakeValueArgs makeValueArgs) : base(makeValueArgs) { }

    public static implicit operator T(Option<T> value) => (T)(object)Convert.ToInt32(value.Primitive);

    public static implicit operator Option<T>(T value) => Option.Of(value);

    public override IValueProvider MakeOfThisType(MakeValueArgs args) => new Number(args);

    public override IValueProvider MakeDefault() => new Option<T>(default(T));

    public override bool Equals(object? obj) => Equals(obj as IValueProvider);

    public override int GetHashCode() => base.GetHashCode();

    public SwitchExpression<T, TResult>.SwichBuilder Switch<TResult>()
        where TResult : class, IValueProvider, new() => SwitchExpression<T, TResult>.For(this);

    public static Condition operator ==(Option<T>? left, Option<T>? right) => Enforce.NotNull(left).IsEqualToRight(right);

    public static Condition operator !=(Option<T>? left, Option<T>? right) => Enforce.NotNull(left).NotEqualToRight(right);

    private Condition IsEqualToRight(Option<T>? right) => HandleComparisonOperation(Enforce.NotNull(right), (a, b) => a == b);

    private Condition NotEqualToRight(Option<T>? right) => HandleComparisonOperation(Enforce.NotNull(right), (a, b) => a != b);

    private Condition HandleComparisonOperation(IValueProvider value, Func<decimal, decimal, bool> compareFunc, [CallerMemberName] string operatorName = StringConstants.NaN) =>
        HandleBinaryOperation<Condition, bool>(value, (a, b) => compareFunc(a.Primitive, b.Primitive), operatorName);
}
