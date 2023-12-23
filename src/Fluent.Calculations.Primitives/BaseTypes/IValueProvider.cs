namespace Fluent.Calculations.Primitives.BaseTypes;

/// <include file="Docs.xml" path='*/IValueProvider/class/*'/>
public interface IValueProvider : IValue
{
    /// <include file="Docs.xml" path='*/IValueProvider/MakeOfThisType/*'/>
    IValueProvider MakeOfThisType(MakeValueArgs args);

    /// <include file="Docs.xml" path='*/IValueProvider/MakeDefault/*'/>
    IValueProvider MakeDefault();

    /// <include file="Docs.xml" path='*/IValueProvider/Accept/*'/>
    IValue Accept(ValueVisitor visitor);
}
