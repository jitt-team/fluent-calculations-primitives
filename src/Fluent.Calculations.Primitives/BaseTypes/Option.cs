using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Utils;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Fluent.Calculations.Primitives.BaseTypes
{
    /// <include file="Docs.xml" path='*/Option/class/*'/>
    public class Option<T> : Value,
        IEqualityOperators<Option<T>, Option<T>, Condition>
        where T : struct, Enum
    {
        internal Option() : this(MakeValueArgs.Compose(StringConstants.Zero, new ExpressionNode(StringConstants.ZeroDigit, ExpressionNodeType.Constant), 0)) { }

        internal Option(Option<T> enumValue) : base(enumValue) { }

        internal Option(MakeValueArgs makeValueArgs) : base(makeValueArgs) { }

        /// <include file="Docs.xml" path='*/Option/implicit-option/*'/>
        public static implicit operator T(Option<T> value) => (T)(object)Convert.ToInt32(value.Primitive);

        /// <include file="Docs.xml" path='*/Option/implicit-T/*'/>
        public static implicit operator Option<T>(T value) => Option.Of(value);

        /// <include file="Docs.xml" path='*/Option/MakeOfThisType/*'/>
        public override IValueProvider MakeOfThisType(MakeValueArgs args) => new Option<T>(args);

        /// <include file="Docs.xml" path='*/Option/MakeDefault/*'/>
        public override IValueProvider MakeDefault() => new Option<T>(default(T));

        /// <include file="Docs.xml" path='*/Option/Equals/*'/>
        public override bool Equals(object? obj) => Equals(obj as IValueProvider);

        /// <include file="Docs.xml" path='*/Option/GetHashCode/*'/>
        public override int GetHashCode() => base.GetHashCode();

        /// <include file="Docs.xml" path='*/Option/PrimitiveString/*'/>
        public override string PrimitiveString => Enum.GetName(typeof(T), Convert.ToInt32(Primitive)) ?? StringConstants.NaN;

        /// <include file="Docs.xml" path='*/Option/Switch/*'/>
        public SwitchExpression<T, TResult>.SwitchBuilder Switch<TResult>() where TResult : class, IValueProvider, new() => SwitchExpression<T, TResult>.For(this);

        /// <include file="Docs.xml" path='*/Option/operator-equal/*'/>
        public static Condition operator ==(Option<T>? left, Option<T>? right) => Enforce.NotNull(left).IsEqualToRight(right);

        /// <include file="Docs.xml" path='*/Option/operator-not-equal/*'/>
        public static Condition operator !=(Option<T>? left, Option<T>? right) => Enforce.NotNull(left).NotEqualToRight(right);

        private Condition IsEqualToRight(Option<T>? right) => HandleBinaryComparisonOperation(Enforce.NotNull(right), (a, b) => a == b);

        private Condition NotEqualToRight(Option<T>? right) => HandleBinaryComparisonOperation(Enforce.NotNull(right), (a, b) => a != b);

        private Condition HandleBinaryComparisonOperation(IValueProvider value, Func<decimal, decimal, bool> compareFunc, [CallerMemberName] string operatorName = StringConstants.NaN) =>
            HandleBinaryOperation<Condition, bool>(value, (a, b) => compareFunc(a.Primitive, b.Primitive), operatorName);
    }

    /// <include file="Docs.xml" path='*/Option-static/class/*'/>
    public static class Option
    {
        /// <include file="Docs.xml" path='*/Option-static/Of/*'/>
        public static Option<TEnum> Of<TEnum>(TEnum primitiveValue, [CallerMemberName] string fieldName = StringConstants.NaN) where TEnum : struct, Enum
            => new(MakeValueArgs.Compose(fieldName, new ExpressionNode($"{primitiveValue}", ExpressionNodeType.Constant),
                Convert.ToDecimal((int)(object)primitiveValue)) /* explore if this cast can be optimized */);
    }
}
