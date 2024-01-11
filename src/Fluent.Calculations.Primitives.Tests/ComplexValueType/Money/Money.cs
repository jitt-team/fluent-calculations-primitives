namespace Fluent.Calculations.Primitives.Tests.ComplexValueType;
using Fluent.Calculations.Primitives.BaseTypes;

public class Money : Number
{
    public override string ToString() => $"{base.ToString()} ({Currency})";

    public Currency Currency { get; init; }

    public Money() : base() => Currency = Currency.None;

    public Money(Currency currency) : this(Zero, currency) { }

    public Money(Number value, Currency currency) : base(value) => Currency = currency;

    public Money(Money money) : this(money, money.Currency) { }

    public Money(MakeValueArgs makeValueArgs, Currency currency) : base(makeValueArgs) => Currency = currency;

    public static Money operator +(Money left, Money right) => new(left.Addition(right), left.Currency);

    public static Money operator *(Money left, Number right) => new(left.Multiply(right), left.Currency);

    public static Money operator /(Number left, Money right) => new(right.Division(left), right.Currency);

    public override IValueProvider MakeOfThisType(MakeValueArgs args) => new Money(args, Currency);

    public override IValueProvider MakeDefault() => new Money(Currency.None);
}

