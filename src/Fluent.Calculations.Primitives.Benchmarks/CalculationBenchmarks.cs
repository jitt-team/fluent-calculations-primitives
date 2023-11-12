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
            additionContext = new BasicAdditionWithContext();
            additionNative = new BasicAdditionNative();
        }

        [Benchmark]
        public void Calculate_UsingContext()
        {
            Number result = additionContext.ToResult();
        }

        [Benchmark]
        public void Calculate_Native()
        {
            decimal result = additionNative.Return();
        }
    }

    public class BasicAdditionWithContext : EvaluationContext<Number>
    {
        public BasicAdditionWithContext() : base(new EvaluationOptions { AlwaysReadNamesFromExpressions = false }) { }

        public Number
            ValueOne = Number.Of(10),
            ValueTwo = Number.Of(20);

        Number Result => Evaluate(() => ValueOne + ValueTwo);

        public override Number Return() => Result;
    }

    public class BasicAdditionNative
    {
        public decimal
            ValueOne = 10,
            ValueTwo = 20;

        decimal Result => ValueOne + ValueTwo;

        public decimal Return() => Result;
    }
}
