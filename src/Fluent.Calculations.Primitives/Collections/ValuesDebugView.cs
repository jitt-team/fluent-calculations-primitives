namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Diagnostics;

/// <include file="Docs.xml" path='*/ValuesDebugView/class/*'/>
public class ValuesDebugView
{
    private readonly IValue collectionValue;

    /// <include file="Docs.xml" path='*/ValuesDebugView/ctor/*'/>
    public ValuesDebugView(IValue collectionValue) => this.collectionValue = collectionValue;

    /// <include file="Docs.xml" path='*/ValuesDebugView/Items/*'/>
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public IValue[] Items => collectionValue.Expression.Arguments.ToArray();
}