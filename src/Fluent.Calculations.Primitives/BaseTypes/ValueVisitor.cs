namespace Fluent.Calculations.Primitives.BaseTypes;

public class ValueVisitor
{
    protected virtual void Visit(IValue value) => ((IValueProvider)value).Accept(this);

    public virtual void VisitArgument(IValue value) => Visit(value);
}
