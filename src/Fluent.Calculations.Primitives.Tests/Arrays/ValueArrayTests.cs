﻿using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Collections;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Arrays
{
    public static class Constant
    {
        public const string TestEvaluationName = "TEST-ARRAY-EVALUATION-NAME";
    }

    public partial class ValueArrayTests
    {
        [Fact]
        public void Test()
        {
            var result = TestArrayCalcuation.Return();
            result.Should().NotBeNull();
            result.Primitive.Should().Be(5);
        }

        public class TestArrayCalcuation
        {
            public static Number Return()
            {
                EvaluationOptions options = new() { AlwaysReadNamesFromExpressions = true };
                EvaluationContext<Number> Calculation = new(options);

                Number
                    NumberOne = Number.Of(2, nameof(NumberOne)),
                    NumberTwo = Number.Of(3, nameof(NumberTwo));

                Values<Number> MultipleNumbers = Values<Number>.Of(() => new[] { NumberOne, NumberTwo }, nameof(MultipleNumbers));

                return Calculation.Evaluate(() => MultipleNumbers.Sum(), Constant.TestEvaluationName);
            }

        }
    }
}