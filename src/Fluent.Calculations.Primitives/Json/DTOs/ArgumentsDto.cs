namespace Fluent.Calculations.Primitives.Json;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Collections.Generic;
using System.Diagnostics;

[DebuggerDisplay("Count = {Count}")]
internal sealed class ArgumentsDto : List<IValue>, IArguments { }
