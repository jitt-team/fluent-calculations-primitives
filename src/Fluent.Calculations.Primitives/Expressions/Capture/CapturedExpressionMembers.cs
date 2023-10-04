namespace Fluent.Calculations.Primitives.Expressions.Capture;

public class CapturedExpressionMembers
{
    internal CapturedParameterMember[] Parameters { get; }

    internal CapturedEvaluationMember[] Evaluations { get; }

    internal CapturedExpressionMembers(IEnumerable<CapturedParameterMember> parameterMembers, IEnumerable<CapturedEvaluationMember> evaluationMembers)
    {
        Parameters = parameterMembers.ToArray();
        Evaluations = evaluationMembers.ToArray();
    }
}