using DotNetGraph.Core;
using DotNetGraph.Extensions;
using Fluent.Calculations.DotNetGraph.Shared;
using Fluent.Calculations.DotNetGraph.Styles;
using Fluent.Calculations.Primitives.BaseTypes;
namespace Fluent.Calculations.DotNetGraph;

public class DotGraphValueBuilder
{
    private readonly IGraphStyle builder;

    public DotGraphValueBuilder(IGraphStyle builder) => this.builder = builder;

    public DotGraphValueBuilder() : this(new DotNetGraphBuilderStyle1()) { }

    public DotGraph Build(IValue value)
    {
        DotGraph mainGraph = CreateDirectedGraph("FluentCalculations");
        DotSubgraph paramsGraph = builder.CreateParametersCluster();
        mainGraph.Add(paramsGraph);

        AddToGraph(value, mainGraph, paramsGraph);

        return mainGraph;
    }

    public DotGraph CreateDirectedGraph(string identifier) =>new DotGraph().WithIdentifier(identifier).Directed();

    private DotNodeBlock AddToGraph(IValue value, DotGraph mainGraph, DotSubgraph paramsGraph)
    {
        DotNodeBlock parentNode = builder.CreateBlock(value);
        DotBaseGraph graph = IsParameter() ? paramsGraph : mainGraph;
        graph.AddRange(parentNode);

        foreach (IValue argument in value.Expression.Arguments)
        {
            DotNodeBlock child = AddToGraph(argument, mainGraph, paramsGraph);
            DotEdge edge = builder.ConnectValues(parentNode.LastNode, child.FirstNode);
            mainGraph.Add(edge);
        }

        return parentNode;

        bool IsParameter() => value.Origin == ValueOriginType.Parameter || value.Origin == ValueOriginType.Constant;
    }
}
