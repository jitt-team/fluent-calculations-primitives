namespace Fluent.Calculations.Primitives.Expressions.Capture;

internal class CapturedExpressionValues
{
    public CapturedParameter[] Parameters { get; }

    public CapturedEvaluation[] Evaluations { get; }

    public CapturedExpressionValues(IEnumerable<CapturedParameter> inputParameters, IEnumerable<CapturedEvaluation> evaluationPointer)
    {
        Parameters = inputParameters.ToArray();
        Evaluations = evaluationPointer.ToArray();
    }
}