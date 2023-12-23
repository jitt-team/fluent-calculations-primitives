namespace Fluent.Calculations.Primitives.BaseTypes;
using System.Collections;
using System.Diagnostics;

/// <include file="Docs/IntelliSense.xml" path='docs/members[@name="TagsCollection"]/class/*' />
[DebuggerDisplay("Count = {Count}")]
public sealed class TagsCollection : ITags
{
    private readonly List<Tag> items;

    private TagsCollection() => items = new List<Tag>();

    internal TagsCollection(IEnumerable<Tag> tags) => this.items = new List<Tag>(tags);

    internal static TagsCollection Empty => new TagsCollection();

    public int Count => items.Count;

    public IEnumerator<Tag> GetEnumerator() => items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();
}
