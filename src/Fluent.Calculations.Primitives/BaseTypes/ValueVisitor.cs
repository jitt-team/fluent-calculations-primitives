namespace Fluent.Calculations.Primitives.BaseTypes;

/// <include file="Docs.xml" path='*/ValueVisitor/class/*' />
public abstract class ValueVisitor
{
    protected virtual void Visit(IValue value) => ((IValueProvider)value).Accept(this);

    public virtual void VisitArgument(IValue value) => Visit(value);
}
