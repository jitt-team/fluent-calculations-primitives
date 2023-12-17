namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;

internal class ValueArgumentsSelector : ValueVisitor, IValueArgumentsSelector
{
    private readonly Dictionary<string, IValue> evaluationsAndParameters = new(5);

    public IValue[] Select(IValue value)
    {
        if(IsOperationResult(value))
            Visit(value);
        else
            evaluationsAndParameters.TryAdd(value.Name, value);

        IValue[] arguments = [.. evaluationsAndParameters.Values];

        evaluationsAndParameters.Clear();

        return arguments;
    }

    public override void VisitArgument(IValue value)
    {
        if (IsOperationResult(value))
            base.VisitArgument(value);
        else
            evaluationsAndParameters.TryAdd(value.Name, value);
    }

    private static bool IsOperationResult(IValue value) => value.Origin == ValueOriginType.Operation || value.Origin == ValueOriginType.NaN;

}
