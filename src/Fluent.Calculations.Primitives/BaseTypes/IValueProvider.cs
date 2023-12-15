namespace Fluent.Calculations.Primitives.BaseTypes;

public interface IValueProvider : IValue
{
    IValueProvider MakeOfThisType(MakeValueArgs args);

    IValueProvider MakeDefault();

    IValue Accept(ValueVisitor visitor);
}
