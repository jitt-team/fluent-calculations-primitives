namespace Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;

internal class ValueArgumentsSelector : ValueVisitor, IValueArgumentsSelector
{
    private readonly Dictionary<string, IValue> arguments = new(5);

    public IValue[] SelectArguments(IValue value)
    {
        Visit(value);

        return [.. arguments.Values];
    }

    public override void VisitArgument(IValue value) => base.VisitArgument(CaptureIgnoreOperationResults(value));

    private IValue CaptureIgnoreOperationResults(IValue value)
    {
        if (value.Origin != ValueOriginType.Operation && value.Origin != ValueOriginType.NaN)
            arguments.TryAdd(value.Name, value);

        return value;
    }
}
