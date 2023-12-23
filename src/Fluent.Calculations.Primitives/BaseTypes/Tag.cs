using System.Diagnostics;

namespace Fluent.Calculations.Primitives.BaseTypes;

/// <include file="Docs.xml" path='*/Tag/class/*'/>
[DebuggerDisplay("{Name}")]
public sealed class Tag
{
    /// <include file="Docs.xml" path='*/Tag/Name/*'/>
    public required string Name { get; init; }

    private Tag() { }

    /// <include file="Docs.xml" path='*/Tag/Create/*'/>
    public static Tag Create(string name) => new() { Name = name };
}