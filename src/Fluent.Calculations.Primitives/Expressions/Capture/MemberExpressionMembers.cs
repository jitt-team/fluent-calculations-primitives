namespace Fluent.Calculations.Primitives.Expressions.Capture;

public class MemberExpressionMembers
{
    internal CapturedParameterMember[] Parameters { get; }

    internal CapturedEvaluationMember[] Evaluations { get; }

    internal MemberExpressionMembers(IEnumerable<CapturedParameterMember> parameterMembers, IEnumerable<CapturedEvaluationMember> evaluationMembers)
    {
        Parameters = parameterMembers.ToArray();
        Evaluations = evaluationMembers.ToArray();
    }
}