namespace Fluent.Calculations.Primitives.BaseTypes;
using System.Diagnostics;

/// <include file="Docs.xml" path='*/ArgumentsDebugView/class/*' />
public class ArgumentsDebugView
{
    private readonly IArguments arguments;

    public ArgumentsDebugView(IArguments arguments) => this.arguments = arguments;

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public IValue[] Arguments => arguments.ToArray();
}