namespace Fluent.Calculations.Primitives.Expressions.Capture;

internal class CapturedEvaulation
{
    public string Name { get; }

    public CapturedEvaulation(string name) => Name = name;

    internal static bool OfType(object obj) => obj is CapturedEvaulation;
}