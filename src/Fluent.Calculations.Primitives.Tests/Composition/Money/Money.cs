namespace Fluent.Calculations.Primitives.Tests.Composition;
using Fluent.Calculations.Primitives.BaseTypes;

public class Money : Number
{
    public override string ToString() => $"{base.ToString()} ({Currency})";

    public Currency Currency { get; init; }

    public Money() : base() => Currency = Currency.None;

    public Money(Currency currency) : this(Zero, currency) { }

    public Money(Number value, Currency currency) : base(value) => Currency = currency;

    public Money(Money money) : this(money, money.Currency) { }

    public Money(CreateValueArgs createValueArgs, Currency currency) : base(createValueArgs) => Currency = currency;

    public static Money operator +(Money left, Money right) => new Money(left.Add(right), left.Currency);

    public override IValue Compose(CreateValueArgs args) => new Money(args, Currency);

    public override IValue Default => new Money(Currency.None);
}

