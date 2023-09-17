namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;

internal class CapturedParameter
{
    public IValue Value { get; }

    public string Name { get; }

    public CapturedParameter(IValue value, string name)
    {
        Value = value;
        Name = name;
    }

    internal static bool IsOfType(object obj) => obj is CapturedParameter;
}