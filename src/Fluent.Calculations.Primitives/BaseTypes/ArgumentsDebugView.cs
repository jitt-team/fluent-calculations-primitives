namespace Fluent.Calculations.Primitives.BaseTypes;
using System.Diagnostics;

/// <include file="Docs.xml" path='*/ArgumentsDebugView/class/*'/>
/// <include file="Docs.xml" path='*/ArgumentsDebugView/ctor-arguments/*'/>
public class ArgumentsDebugView(IArguments arguments)
{
    private readonly IArguments arguments = arguments;

    /// <include file="Docs.xml" path='*/ArgumentsDebugView/Arguments/*'/>
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public IValue[] Arguments => arguments.ToArray();
}