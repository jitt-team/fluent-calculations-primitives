namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;

public class CapturedParameterMember
{
    internal IValue Value { get; }

    internal string MemberName { get; }

    internal CapturedParameterMember(IValue value, string fieldOrPropertyName)
    {
        Value = value;
        MemberName = fieldOrPropertyName;
    }
}