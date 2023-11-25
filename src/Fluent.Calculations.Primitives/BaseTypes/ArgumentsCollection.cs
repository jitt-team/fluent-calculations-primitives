namespace Fluent.Calculations.Primitives.BaseTypes;
using System.Collections;
using System.Diagnostics;

[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(ArgumentsDebugView))]
public sealed class ArgumentsCollection : IArguments
{
    private readonly List<IValueMetadata> items;

    private ArgumentsCollection() => items = new List<IValueMetadata>();

    internal ArgumentsCollection(IEnumerable<IValueMetadata> arguments) => items = new List<IValueMetadata>(arguments);

    internal static ArgumentsCollection Empty => new ArgumentsCollection();

    public int Count => items.Count;

    internal static ArgumentsCollection CreateFrom(IValueMetadata[] arguments) => new ArgumentsCollection(arguments);

    public IEnumerator<IValueMetadata> GetEnumerator() => items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();

    internal void AddRange(IArguments arguments) => items.AddRange(arguments);

    internal void Add(IValueMetadata arguments) => items.Add(arguments);
}