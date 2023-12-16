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

    public DotGraphValueBuilder() : this(new GraphStyleMonochrome()) { }

    public DotGraph Build(IValue value)
    {
        DotGraph mainGraph = CreateDirectedGraph("FluentCalculations");
        ClusterProvider parameterClustersProvider = new ClusterProvider(builder, mainGraph);

        AddToGraph(value, mainGraph, parameterClustersProvider);

        return mainGraph;
    }

    public static DotGraph CreateDirectedGraph(string identifier) => new DotGraph().WithIdentifier(identifier).Directed();

    private DotNodeBlock AddToGraph(IValue value, DotGraph mainGraph, ClusterProvider parameterClustersProvider)
    {
        DotNodeBlock parentNode = builder.CreateBlock(value);
        DotBaseGraph graph = IsParameter() ? parameterClustersProvider.GetOrCreate(value.Scope) : mainGraph;
        graph.AddRange(parentNode);

        foreach (IValue argument in value.Expression.Arguments)
        {
            DotNodeBlock child = AddToGraph(argument, mainGraph, parameterClustersProvider);
            DotEdge edge = builder.ConnectValues(parentNode.LastNode, child.FirstNode);
            mainGraph.Add(edge);
        }

        return parentNode;

        bool IsParameter() => value.Origin == ValueOriginType.Parameter || value.Origin == ValueOriginType.Constant;
    }

    private class ClusterProvider
    {
        private readonly Dictionary<string, DotSubgraph> scopeClusters = new Dictionary<string, DotSubgraph>();

        private readonly IGraphStyle builder;
        private DotGraph mainGraph;

        public ClusterProvider(IGraphStyle builder, DotGraph mainGraph)
        {
            this.mainGraph = mainGraph;
            this.builder = builder;
        }

        public DotSubgraph GetOrCreate(string scope)
        {
            DotSubgraph subgraph;

            if (scopeClusters.TryGetValue(scope, out subgraph))
                return subgraph;

            int clusterCount = scopeClusters.Count - 1;
            subgraph = builder.CreateParametersCluster(scope, clusterCount + 1);
            scopeClusters.Add(scope, subgraph);
            mainGraph.Add(subgraph);

            return subgraph;
        }
    }
}
