namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;

public class CapturedParameterMember
{
    internal IValueProvider Value { get; }

    internal string MemberName { get; }

    internal CapturedParameterMember(IValueProvider value, string fieldOrPropertyName)
    {
        Value = value;
        MemberName = fieldOrPropertyName;
    }
}