namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Diagnostics;

/// <include file="Docs/IntelliSense.xml" path='docs/members[@name="ValuesDebugView"]/class/*' />
public class ValuesDebugView
{
    private readonly IValue collectionValue;

    public ValuesDebugView(IValue collectionValue) => this.collectionValue = collectionValue;

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public IValue[] Items => collectionValue.Expression.Arguments.ToArray();
}