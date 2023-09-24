namespace Fluent.Calculations.Primitives.Expressions.Capture;

public class CapturedEvaluationMember
{
    internal string MemberName { get; }

    internal CapturedEvaluationMember(string memberName) => MemberName = memberName;
}