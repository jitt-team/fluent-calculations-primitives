namespace Fluent.Calculations.Primitives.BaseTypes;

/// <include file="IntelliSense.xml" path='docs/members[@name="ArgumentsVisitorInvoker"]/class/*' />
public static class ArgumentsVisitorInvoker
{ 
    public static IValue VisitArguments(IValue value, ValueVisitor visitor)
    {
        foreach (IValue argument in value.Expression.Arguments)
            visitor.VisitArgument(argument);

        return value;
    }
}