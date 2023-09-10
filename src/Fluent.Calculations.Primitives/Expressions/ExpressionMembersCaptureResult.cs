namespace Fluent.Calculations.Primitives.Expressions
{
    internal class ExpressionMembersCaptureResult 
    {
        public CapturedInputParameter[] InputParameters { get; }

        public CapturedEvaulation[] EvaluationPointers { get; }

        public ExpressionMembersCaptureResult(CapturedInputParameter[] inputParameters, CapturedEvaulation[] evaluationPointer)
        {
            this.InputParameters = inputParameters;
            this.EvaluationPointers = evaluationPointer;
        }
    }
}