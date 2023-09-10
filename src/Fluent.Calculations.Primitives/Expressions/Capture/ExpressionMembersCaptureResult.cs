namespace Fluent.Calculations.Primitives.Expressions.Capture
{
    internal class ExpressionMembersCaptureResult
    {
        public CapturedInputParameter[] InputParameters { get; }

        public CapturedEvaulation[] EvaluationPointers { get; }

        public ExpressionMembersCaptureResult(CapturedInputParameter[] inputParameters, CapturedEvaulation[] evaluationPointer)
        {
            InputParameters = inputParameters;
            EvaluationPointers = evaluationPointer;
        }
    }
}