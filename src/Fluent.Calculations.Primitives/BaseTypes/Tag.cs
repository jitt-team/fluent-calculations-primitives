namespace Fluent.Calculations.Primitives.BaseTypes;
using System.Diagnostics;

/// <include file="Docs.xml" path='*/Tag/class/*'/>
[DebuggerDisplay("{Name}")]
public sealed class Tag
{
    /// <include file="Docs.xml" path='*/Tag/Name/*'/>
    public required string Name { get; init; }

    private Tag() { }

    /// <include file="Docs.xml" path='*/Tag/Create/*'/>
    public static Tag Create(string name) => new() { Name = name };

    /// <include file="Docs.xml" path='*/Tag/ToString/*'/>
    public override string ToString() => Name;
}