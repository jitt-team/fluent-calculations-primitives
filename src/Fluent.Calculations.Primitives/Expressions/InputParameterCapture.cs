namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;

internal class InputParameterCapture
{
    public IValue Value { get; }

    public string Name { get; }

    public InputParameterCapture(IValue value, string name)
    {
        this.Value = value;
        this.Name = name;
    }

    internal static bool OfType(object obj) => obj is InputParameterCapture;
}