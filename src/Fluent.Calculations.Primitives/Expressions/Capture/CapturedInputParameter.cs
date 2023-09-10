namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;

internal class CapturedInputParameter
{
    public IValue Value { get; }

    public string Name { get; }

    public CapturedInputParameter(IValue value, string name)
    {
        Value = value;
        Name = name;
    }

    internal static bool OfType(object obj) => obj is CapturedInputParameter;
}