namespace Fluent.Calculations.Primitives.BaseTypes;
using System.Collections;
using System.Diagnostics;

[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(ArgumentsDebugView))]
public sealed class ArgumentsCollection : IArguments
{
    private readonly List<IValue> items;

    private ArgumentsCollection() => items = new List<IValue>();

    internal ArgumentsCollection(IEnumerable<IValue> arguments) => items = new List<IValue>(arguments);

    internal static ArgumentsCollection Empty => new ArgumentsCollection();

    public int Count => items.Count;

    internal static ArgumentsCollection CreateFrom(IValue[] arguments) => new ArgumentsCollection(arguments);

    public IEnumerator<IValue> GetEnumerator() => items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();

    internal void AddRange(IArguments arguments) => items.AddRange(arguments);

    internal void Add(IValue arguments) => items.Add(arguments);
}