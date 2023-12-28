namespace Fluent.Calculations.Primitives.BaseTypes;

/// <include file="Docs.xml" path='*/ArgumentsVisitorInvoker/class/*'/>
public static class ArgumentsVisitorInvoker
{
    /// <include file="Docs.xml" path='*/ArgumentsVisitorInvoker/VisitArguments/*'/>
    public static IValue VisitArguments(IValue value, ValueVisitor visitor)
    {
        foreach (IValue argument in value.Expression.Arguments)
            visitor.VisitArgument(argument);

        return value;
    }
}