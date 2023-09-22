namespace Fluent.Calculations.Primitives.Expressions.Capture;

public class MemberExpressionValues
{
    internal CapturedParameter[] Parameters { get; }

    internal CapturedEvaluation[] Evaluations { get; }

    internal MemberExpressionValues(IEnumerable<CapturedParameter> inputParameters, IEnumerable<CapturedEvaluation> evaluationPointer)
    {
        Parameters = inputParameters.ToArray();
        Evaluations = evaluationPointer.ToArray();
    }
}