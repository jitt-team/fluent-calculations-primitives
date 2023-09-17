namespace Fluent.Calculations.Primitives.Expressions.Capture;

internal class CapturedExpressionValues
{
    public CapturedParameter[] Parameters { get; }

    public CapturedEvaluation[] Evaluations { get; }

    public CapturedExpressionValues(CapturedParameter[] inputParameters, CapturedEvaluation[] evaluationPointer)
    {
        Parameters = inputParameters;
        Evaluations = evaluationPointer;
    }
}