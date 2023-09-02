namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;

public interface IValue
{
    string Name { get; }

    decimal PrimitiveValue { get; }

    bool IsConstant { get; }

    bool IsEndResult { get; }

    ExpressionNode Expression { get; }

    TagsCollection Tags { get; }

    IValue Create(CreateValueArgs args);

    IValue Default { get; }
}
