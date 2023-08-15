namespace Fluent.Calculations.Primitives;

public interface IValue
{
    string Name { get; }

    string Expresion { get; }

    decimal PrimitiveValue { get; }

    bool IsConstant { get; }

    ArgumentsList Arguments { get; }

    TagsList Tags { get; }

    abstract IValue ToExpressionResult(CreateValueArgs args);
}
