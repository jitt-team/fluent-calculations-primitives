﻿namespace Fluent.Calculations.Primitives.Tests.ComplexValueType;
using Fluent.Calculations.Primitives.BaseTypes;

public class Money : Number
{
    public override string ToString() => $"{base.ToString()} ({Currency})";

    public Currency Currency { get; init; }

    public Money() : base() => Currency = Currency.None;

    public Money(Currency currency) : this(Zero, currency) { }

    public Money(Number value, Currency currency) : base(value) => Currency = currency;

    public Money(Money money) : this(money, money.Currency) { }

    public Money(MakeValueArgs createValueArgs, Currency currency) : base(createValueArgs) => Currency = currency;

    public static Money operator +(Money left, Money right) => new Money(left.Add(right), left.Currency);

    public static Money operator *(Money left, Number right) => new Money(left.Multiply(right), left.Currency);

    public static Money operator *(Number left, Money right) => new Money(right.Multiply(left), right.Currency);

    public override IValueProvider MakeOfThisType(MakeValueArgs args) => new Money(args, Currency);

    public override IValueProvider MakeDefault() => new Money(Currency.None);
}

