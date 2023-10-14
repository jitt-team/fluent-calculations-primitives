using DotNetGraph.Core;

namespace Fluent.Calculations.DotNetGraph;

public class DotNodeBlock
{
    public bool IsValuePart { get; set; }
    public DotNode FirstNode { get; set; }
    public DotEdge ConnectorEdge { get; set; }
    public DotNode LastNode { get; set; }
}
