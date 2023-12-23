namespace Fluent.Calculations.Primitives.BaseTypes;

/// <include file="Docs.xml" path='*/ValueVisitor/class/*'/>
public abstract class ValueVisitor
{
    /// <include file="Docs.xml" path='*/ValueVisitor/ctor-value/*'/>
    protected virtual void Visit(IValue value) => ((IValueProvider)value).Accept(this);

    /// <include file="Docs.xml" path='*/ValueVisitor/VisitArgument/*'/>
    public virtual void VisitArgument(IValue value) => Visit(value);
}
