namespace Fluent.Calculations.Primitives.Expressions.Capture;
using Fluent.Calculations.Primitives.BaseTypes;

public class CapturedParameter
{
    internal IValue Value { get; }

    internal string Name { get; }

    internal CapturedParameter(IValue value, string name)
    {
        Value = value;
        Name = name;
    }
}