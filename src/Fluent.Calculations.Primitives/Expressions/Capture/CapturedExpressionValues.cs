namespace Fluent.Calculations.Primitives.Expressions.Capture;

public class CapturedExpressionValues
{
    internal CapturedParameter[] Parameters { get; }

    internal CapturedEvaluation[] Evaluations { get; }

    internal CapturedExpressionValues(IEnumerable<CapturedParameter> inputParameters, IEnumerable<CapturedEvaluation> evaluationPointer)
    {
        Parameters = inputParameters.ToArray();
        Evaluations = evaluationPointer.ToArray();
    }
}