namespace Fluent.Calculations.Primitives.Tests.Evaluation;
using Fluent.Calculations.Primitives.BaseTypes;

public class EvaluationContextTests
{
    [Fact]
    public void Test()
    {
        Number 
            NumberOne = Number.Of(5, "TEST-NUMBER-ONE"),
            NumberTwo = Number.Of(3, "TEST-NUMBER-TWO");

        var calculation = BuildCalculation();

        

        Number result = calculation.Evaluate(() => NumberOne * NumberTwo);
    }

    private EvaluationContext<Number> BuildCalculation()
    {

        EvaluationContext<Number> calculation = new EvaluationContext<Number>(valueCache, expressionCapturer);
        return calculation;
    }
}
