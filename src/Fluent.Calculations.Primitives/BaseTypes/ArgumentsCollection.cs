namespace Fluent.Calculations.Primitives.BaseTypes;
using System.Collections;
using System.Diagnostics;

[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(ArgumentsCollectionDebugView))]
public sealed class ArgumentsCollection : IReadOnlyCollection<IValue>
{
    private readonly List<IValue> items;

    private ArgumentsCollection() => items = new List<IValue>();

    internal ArgumentsCollection(IEnumerable<IValue> arguments) => items = new List<IValue>(arguments);

    internal static ArgumentsCollection Empty => new ArgumentsCollection();

    public int Count => items.Count;

    internal static ArgumentsCollection CreateFrom(IValue[] arguments) => new ArgumentsCollection(arguments);

    public IEnumerator<IValue> GetEnumerator() => items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();

    internal void AddRange(ArgumentsCollection arguments) => items.AddRange(arguments);

    internal void Add(IValue arguments) => items.Add(arguments);
}