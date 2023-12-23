namespace Fluent.Calculations.Primitives.BaseTypes;

/// <include file="Docs.xml" path='*/IArguments/class/*' />
public interface IArguments : IEnumerable<IValue>
{
    public int Count { get; }
}