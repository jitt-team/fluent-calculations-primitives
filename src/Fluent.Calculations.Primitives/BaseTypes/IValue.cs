namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;

public interface IValue
{
    string Name { get; }

    decimal Primitive { get; }

    bool IsParameter { get; }

    bool IsOutput { get; }

    ExpressionNode Expression { get; }

    TagsCollection Tags { get; }

    IValue MakeOfThisType(MakeValueArgs args);

    IValue GetDefault();

    string ValueToString();
}