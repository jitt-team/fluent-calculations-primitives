namespace Fluent.Calculations.Primitives.BaseTypes;

/// <include file="Docs.xml" path='*/IValueProvider/class/*' />
public interface IValueProvider : IValue
{
    IValueProvider MakeOfThisType(MakeValueArgs args);

    IValueProvider MakeDefault();

    IValue Accept(ValueVisitor visitor);
}
