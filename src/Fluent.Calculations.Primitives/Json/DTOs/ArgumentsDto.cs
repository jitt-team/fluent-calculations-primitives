namespace Fluent.Calculations.Primitives.Json;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Diagnostics;

[DebuggerDisplay("Count = {Count}")]
internal sealed class ArgumentsDto : List<ValueDto>, IArguments
{
    IEnumerator<IValueMetadata> IEnumerable<IValueMetadata>.GetEnumerator() => this.Cast<IValueMetadata>().GetEnumerator();
}
