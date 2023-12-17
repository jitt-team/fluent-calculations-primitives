using DotNetGraph.Core;
using DotNetGraph.Extensions;
using Fluent.Calculations.DotNetGraph.Shared;
using Fluent.Calculations.DotNetGraph.Styles;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Collections;
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
        DotNodeBlock finalResultBlock = AddToGraph(value, mainGraph, parameterClustersProvider);
        AddFinalResultNode(finalResultBlock.FirstNode, mainGraph, value);

        return mainGraph;
    }

    private void AddFinalResultNode(DotNode finalResult, DotGraph mainGraph, IValue value)
    {
        DotNode finalResultValueNode = builder.CreateFinalResult(value);
        DotEdge finalResultEdge = builder.ConnectValues(finalResultValueNode, finalResult);
        mainGraph.Add(finalResultValueNode);
        mainGraph.Add(finalResultEdge);
    }

    public static DotGraph CreateDirectedGraph(string identifier) => new DotGraph().WithIdentifier(identifier).Directed();

    private DotNodeBlock AddToGraph(IValue value, DotGraph mainGraph, ClusterProvider parameterClustersProvider)
    {
        DotNodeBlock parentNode = builder.CreateBlock(value);

        DotBaseGraph graph = IsParameter() ? 
            parameterClustersProvider.GetOrCreateParametersSubgraph(value.Scope) :
            parameterClustersProvider.GetOrCreateScopeSubgraph(value.Scope);

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
        private readonly Dictionary<string, DotSubgraph>
            scopeParameterContainers = new Dictionary<string, DotSubgraph>(),
            scopeContainers = new Dictionary<string, DotSubgraph>();

        private readonly IGraphStyle builder;
        private DotGraph mainGraph;

        public ClusterProvider(IGraphStyle builder, DotGraph mainGraph)
        {
            this.mainGraph = mainGraph;
            this.builder = builder;
        }

        private int GetCurrentScopeIndex() => scopeParameterContainers.Count + scopeParameterContainers.Count - 1;

        public DotSubgraph GetOrCreateScopeSubgraph(string scope)
        {
            DotSubgraph subgraph;

            if (scopeContainers.TryGetValue(scope, out subgraph))
                return subgraph;

            subgraph = builder.CreateScopeCluster(scope, GetCurrentScopeIndex() + 1);
            scopeContainers.Add(scope, subgraph);
            mainGraph.Add(subgraph);

            return subgraph;

        }

        public DotSubgraph GetOrCreateParametersSubgraph(string scope)
        {
            DotSubgraph? scopeParametersSubgraph;

            if (scopeParameterContainers.TryGetValue(scope, out scopeParametersSubgraph))
                return scopeParametersSubgraph;

            scopeParametersSubgraph = builder.CreateParametersCluster(scope, GetCurrentScopeIndex() + 1);
            GetOrCreateScopeSubgraph(scope).Add(scopeParametersSubgraph);
            scopeParameterContainers.Add(scope, scopeParametersSubgraph);

            return scopeParametersSubgraph;
        }
    }
}
