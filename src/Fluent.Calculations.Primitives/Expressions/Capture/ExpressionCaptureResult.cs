namespace Fluent.Calculations.Primitives.Expressions.Capture;

internal class ExpressionCaptureResult
{
    public CapturedInputMember[] InputMembers { get; }

    public CapturedEvaulationMember[] EvaluationMembers { get; }

    public ExpressionCaptureResult(CapturedInputMember[] inputParameters, CapturedEvaulationMember[] evaluationPointer)
    {
        InputMembers = inputParameters;
        EvaluationMembers = evaluationPointer;
    }
}