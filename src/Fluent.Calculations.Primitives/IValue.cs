using Fluent.Calculations.Primitives.Expressions;

namespace Fluent.Calculations.Primitives;

public interface IValue
{
    string Name { get; }

    decimal PrimitiveValue { get; }

    bool IsConstant { get; }

    ExpressionNode Expression { get; }

    TagsList Tags { get; }

    IValue ToExpressionResult(CreateValueArgs args);

    IValue Default { get; }
}
