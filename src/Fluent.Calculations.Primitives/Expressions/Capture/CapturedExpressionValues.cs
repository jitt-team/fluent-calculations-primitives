namespace Fluent.Calculations.Primitives.Expressions.Capture;

internal class CapturedExpressionValues
{
    public CapturedParameter[] InputMembers { get; }

    public CapturedEvaluation[] EvaluationMembers { get; }

    public CapturedExpressionValues(CapturedParameter[] inputParameters, CapturedEvaluation[] evaluationPointer)
    {
        InputMembers = inputParameters;
        EvaluationMembers = evaluationPointer;
    }
}