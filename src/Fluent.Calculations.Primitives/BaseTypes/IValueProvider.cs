namespace Fluent.Calculations.Primitives.BaseTypes;

public interface IValueProvider : IValue
{
    IValueProvider MakeOfThisType(MakeValueArgs args);

    IValueProvider MakeOfThisType(decimal primitiveVale);

    IValueProvider MakeDefault();

    IValue Accept(ValueVisitor visitor);
}
