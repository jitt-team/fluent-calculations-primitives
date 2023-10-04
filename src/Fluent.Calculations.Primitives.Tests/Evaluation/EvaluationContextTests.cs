namespace Fluent.Calculations.Primitives.Tests.Evaluation;
using Fluent.Calculations.Primitives.BaseTypes;

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
