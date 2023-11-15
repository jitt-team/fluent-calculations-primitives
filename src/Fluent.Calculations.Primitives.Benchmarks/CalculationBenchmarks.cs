using BenchmarkDotNet.Attributes;
using Fluent.Calculations.Primitives.BaseTypes;

namespace Fluent.Calculations.Primitives.Tests.Benchmarks
{
    [MemoryDiagnoser(false)]
    public class CalculationBenchmarks
    {
        private readonly BasicAdditionWithContext additionContext;
        private readonly BasicAdditionNative additionNative;

        public CalculationBenchmarks()
        {
            additionContext = new BasicAdditionWithContext
            {
                ConstantOne = Number.Of(2),
                ConstantTwo = Number.Of(10),
                ConstantThree = Number.Of(5),
                ConstantFour = Number.Of(30)
            };
            additionNative = new BasicAdditionNative();
        }

        [Benchmark]
        public void Calculate_UsingContext() => additionContext.ToResult();

        // [Benchmark]
        public void Calculate_Native() => additionNative.Return();
    }

    public class BasicAdditionWithContext : EvaluationContext<Number>
    {
        public BasicAdditionWithContext() : base(new EvaluationOptions { AlwaysReadNamesFromExpressions = false }) { }
        public Number
            ConstantOne = Number.Zero,
            ConstantTwo = Number.Zero,
            ConstantThree = Number.Zero,
            ConstantFour = Number.Zero;

        public Condition ConstantOneGreaterThanTwo => Evaluate(() => ConstantTwo > ConstantThree);
        public Number ConstantOneTimesTwo => Evaluate(() => ConstantOne * ConstantTwo);
        public Number WhenTrueThenValue => Evaluate(() => ConstantOneGreaterThanTwo ? ConstantOneTimesTwo : Number.Zero);
        public Number ConstantFourPlusWhenTrue => Evaluate(() => ConstantFour + WhenTrueThenValue);
        public Number ConstantFourPlusWhenTrue2 => Evaluate(() => ConstantFourPlusWhenTrue + WhenTrueThenValue);
        public Number ConstantFourPlusWhenTrue3 => Evaluate(() => ConstantFour + ConstantFourPlusWhenTrue2);
        public Number ConstantFourPlusWhenTrue4 => Evaluate(() => ConstantFourPlusWhenTrue3 + WhenTrueThenValue);
        public Number ConstantFourPlusWhenTrue5 => Evaluate(() => ConstantFour + ConstantFourPlusWhenTrue4);
        public override Number Return() => ConstantFourPlusWhenTrue5;
    }

    public class BasicAdditionNative
    {
        private decimal
            ValueOne = 10,
            ValueTwo = 20;

        private decimal Result => ValueOne + ValueTwo;

        public decimal Return() => Result;
    }
}
