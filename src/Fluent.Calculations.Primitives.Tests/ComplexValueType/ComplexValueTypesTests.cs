﻿using Fluent.Calculations.Primitives.BaseTypes;
namespace Fluent.Calculations.Primitives.Tests.ComplexValueType;

public class ComplexValueTypesTests
{
    private Money Return()
    {
        EvaluationOptions options = new() { AlwaysReadNamesFromExpressions = true };
        EvaluationContext<Money> Calculation = new EvaluationContext<Money>();

        Money
            MoneyOne = Number.Of(10).AsMoney().EUR,
            MoneyTwo = Number.Of(20).AsMoney().EUR;

        return Calculation.Evaluate(() => MoneyOne + MoneyTwo);
    }
}
