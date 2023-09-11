namespace Fluent.Calculations.Primitives.Expressions.Capture;

internal class CapturedEvaulationMember
{
    public string Name { get; }

    public CapturedEvaulationMember(string name) => Name = name;

    internal static bool OfType(object obj) => obj is CapturedEvaulationMember;
}