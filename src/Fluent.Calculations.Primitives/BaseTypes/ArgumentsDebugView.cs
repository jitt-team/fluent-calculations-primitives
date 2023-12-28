namespace Fluent.Calculations.Primitives.BaseTypes;
using System.Diagnostics;

#pragma warning disable CS1572 // XML comment has a param tag, but there is no parameter by that name
/// <include file="Docs.xml" path='*/ArgumentsDebugView/class/*'/>
/// #pragma warning restore CS1572 // XML comment has a param tag, but there is no parameter by that name
public class ArgumentsDebugView(IArguments arguments)
{
    private readonly IArguments arguments = arguments;

    /// <include file="Docs.xml" path='*/ArgumentsDebugView/Arguments/*'/>
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public IValue[] Arguments => arguments.ToArray();
}