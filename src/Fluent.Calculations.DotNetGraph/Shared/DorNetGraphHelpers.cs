using DotNetGraph.Core;
namespace Fluent.Calculations.DotNetGraph.Shared;

internal static class DorNetGraphHelpers
{
    public static T AddRange<T>(this T graph, IEnumerable<IDotElement> elements) where T : DotBaseGraph
    {
        graph.Elements.AddRange(elements);
        return graph;
    }
}
