namespace Fluent.Calculations.Primitives.Collections;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Diagnostics;

public class ValuesDebugView
{
    private readonly IValue collectionValue;

    public ValuesDebugView(IValue collectionValue) => this.collectionValue = collectionValue;

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public IValue[] Items => collectionValue.Expression.Arguments.ToArray();
}