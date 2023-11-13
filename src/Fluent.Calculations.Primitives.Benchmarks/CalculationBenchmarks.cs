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
        public void Calculate_UsingContext() => additionContext.ToResult();

        [Benchmark]
        public void Calculate_Native() => additionNative.Return();
    }

    public class BasicAdditionWithContext : EvaluationContext<Number>
    {
        public BasicAdditionWithContext() : base(new EvaluationOptions { AlwaysReadNamesFromExpressions = false }) { }

        private Number
            ValueOne = Number.Of(10),
            ValueTwo = Number.Of(20);

        private Number Result => Evaluate(() => ValueOne + ValueTwo);

        public override Number Return() => Result;
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
