namespace Fluent.Calculations.Primitives.Expressions;

internal class PointerToEvaulationCapture
{
    public string Name { get; }

    public PointerToEvaulationCapture(string name) => this.Name = name;

    internal static bool OfType(object obj) => obj is PointerToEvaulationCapture;
}