using Fluent.Calculations.Primitives.BaseTypes;

namespace Fluent.Calculations.Primitives.Tests.Evaluation
{
    public class EvaluationContextTests
    {

        [Fact]
        public void Test()
        {
            EvaluationContext<Number> Calculation = new EvaluationContext<Number>();

            Number NumberOne = Number.Of(5), NumberTwo = Number.Of(3);

            Number result = Calculation.Evaluate(() => NumberOne * NumberTwo);
        }
    }
}
