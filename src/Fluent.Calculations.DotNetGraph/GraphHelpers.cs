using DotNetGraph.Core;
using DotNetGraph.Extensions;

namespace Fluent.Calculations.DotNetGraph;

internal static class GraphHelpers
{


    public static T AddRange<T>(this T graph, params IDotElement[] elements) where T : DotBaseGraph
    {
        graph.Elements.AddRange(elements);
        return graph;
    }
}
