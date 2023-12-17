namespace Fluent.Calculations.Primitives.BaseTypes;

public static class ArgumentsVisitorInvoker
{ 
    public static IValue VisitArguments(IValue value, ValueVisitor visitor)
    {
        foreach (IValue argument in value.Expression.Arguments)
            visitor.VisitArgument(argument);

        return value;
    }
}