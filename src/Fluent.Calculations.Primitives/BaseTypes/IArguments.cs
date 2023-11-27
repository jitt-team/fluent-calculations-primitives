namespace Fluent.Calculations.Primitives.BaseTypes;

public interface IArguments : IEnumerable<IValue>
{
    public int Count { get; }
}