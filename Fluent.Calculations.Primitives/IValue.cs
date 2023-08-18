namespace Fluent.Calculations.Primitives;

public interface IValue
{
    string Name { get; }

    decimal PrimitiveValue { get; }

    bool IsConstant { get; }

    ExpressionNode Expresion { get; }

    TagsList Tags { get; }

    abstract IValue ToExpressionResult(CreateValueArgs args);
}
