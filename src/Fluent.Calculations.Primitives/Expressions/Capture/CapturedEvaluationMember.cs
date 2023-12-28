namespace Fluent.Calculations.Primitives.Expressions.Capture;

internal class CapturedEvaluationMember
{
    internal string MemberName { get; }

    internal CapturedEvaluationMember(string memberName) => MemberName = memberName;
}