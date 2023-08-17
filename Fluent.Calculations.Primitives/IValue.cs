namespace Fluent.Calculations.Primitives;

public interface IValue
{
    string Name { get; }

    ExpressionNode Expresion { get; }

    decimal PrimitiveValue { get; }

    bool IsConstant { get; }

    ArgumentsList Arguments { get; }

    TagsList Tags { get; }

    abstract IValue ToExpressionResult(CreateValueArgs args);
}
