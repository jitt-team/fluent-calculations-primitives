namespace Fluent.Calculations.Primitives.Expressions
{
    internal class ExpressionMembersCaptureResult 
    {
        public InputParameterCapture[] InputParameters { get; }

        public PointerToEvaulationCapture[] EvaluationPointers { get; }

        public ExpressionMembersCaptureResult(InputParameterCapture[] inputParameters, PointerToEvaulationCapture[] evaluationPointer)
        {
            this.InputParameters = inputParameters;
            this.EvaluationPointers = evaluationPointer;
        }
    }
}