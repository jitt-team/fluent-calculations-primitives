namespace Fluent.Calculations.Primitives.Expressions;

internal class CapturedEvaulation
{
    public string Name { get; }

    public CapturedEvaulation(string name) => this.Name = name;

    internal static bool OfType(object obj) => obj is CapturedEvaulation;
}