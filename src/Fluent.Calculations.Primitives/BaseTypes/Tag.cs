using System.Diagnostics;

namespace Fluent.Calculations.Primitives.BaseTypes;

/// <include file="IntelliSense.xml" path='docs/members[@name="Tag"]/class/*' />
[DebuggerDisplay("{Name}")]
public sealed class Tag
{
    public required string Name { get; init; }

    private Tag() { }

    public static Tag Create(string name) => new() { Name = name };
}