namespace Fluent.Calculations.Primitives.Expressions.Capture;

public class CapturedEvaluation
{
    internal string Name { get; }

    internal CapturedEvaluation(string name) => Name = name;

    internal static bool IsOfType(object obj) => obj is CapturedEvaluation;
}