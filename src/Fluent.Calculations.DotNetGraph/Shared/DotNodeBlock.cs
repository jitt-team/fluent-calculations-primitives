using DotNetGraph.Core;

namespace Fluent.Calculations.DotNetGraph;

public class DotNodeBlock
{
    public DotNodeBlock(DotNode firstNode, bool isValuePart)
    {
        this.FirstNode = firstNode;
        this.LastNode = firstNode;
        this.ConnectorEdge = null;
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

    public DotEdge ConnectorEdge { get; set; }

    public DotNode LastNode { get; set; }
}
