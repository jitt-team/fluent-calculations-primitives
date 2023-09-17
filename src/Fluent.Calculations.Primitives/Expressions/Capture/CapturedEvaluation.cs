namespace Fluent.Calculations.Primitives.Expressions.Capture;

internal class CapturedEvaluation
{
    public string Name { get; }

    public CapturedEvaluation(string name) => Name = name;

    internal static bool IsOfType(object obj) => obj is CapturedEvaluation;
}