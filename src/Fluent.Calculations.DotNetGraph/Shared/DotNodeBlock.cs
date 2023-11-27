using DotNetGraph.Core;
using System.Collections;

namespace Fluent.Calculations.DotNetGraph.Shared;

public class DotNodeBlock : IEnumerable<IDotElement>
{
    public DotNodeBlock(DotNode firstNode, bool isValuePart)
    {
        FirstNode = firstNode;
        LastNode = firstNode;
        ConnectorEdge = null;
        IsValuePart = isValuePart;
    }

    public DotNodeBlock(DotNode firstNode, DotNode lastNode, DotEdge connectorEdge)
    {
        FirstNode = firstNode;
        LastNode = lastNode;
        ConnectorEdge = connectorEdge;
    }

    public bool IsValuePart { get; set; }

    public DotNode FirstNode { get; set; }

    public DotEdge? ConnectorEdge { get; set; }

    public DotNode LastNode { get; set; }

    public IEnumerator<IDotElement> GetEnumerator()
    { 
        if(FirstNode != null)
            yield return FirstNode;

        if(ConnectorEdge != null) 
            yield return ConnectorEdge;

        if(LastNode != null) 
            yield return LastNode;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
