using DotNetGraph.Core;
using DotNetGraph.Extensions;

namespace Fluent.Calculations.DotNetGraph;

internal static class GraphBuildingBlocks
{
    public static DotEdge LinkFromValueToArgument(this DotGraph targetGraph, DotNodeBlock valueBlock, DotNodeBlock argumentBlock)
    {
        DotEdge edge = new DotEdge().From(argumentBlock.LastNode)
            .To(valueBlock.FirstNode)
            .WithStyle(DotEdgeStyle.Dashed)
            .WithArrowHead(DotEdgeArrowType.Open);

        targetGraph.Add(edge);

        return edge;
    }

    public static T AddRange<T>(this T graph, params IDotElement[] elements) where T : DotBaseGraph
    {
        graph.Elements.AddRange(elements);
        return graph;
    }
}
