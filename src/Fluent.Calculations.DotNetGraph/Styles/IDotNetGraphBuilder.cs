using DotNetGraph.Core;
using Fluent.Calculations.Primitives.BaseTypes;
namespace Fluent.Calculations.DotNetGraph;

public interface IDotNetGraphBuilder
{
    DotGraph CreateDirectedGraph(string identifier);

    DotSubgraph CreateInputParametersCluster();

    DotNode CreateConsantNode(IValue value);

    DotNode CreateValueNode(IValue value);

    DotNode CreateExpressionNode(IValue value);

    DotEdge CreateSolidEdge(DotNode firstNode, DotNode lastNode);

    DotEdge CreateDashedEdge(DotNode firstNode, DotNode lastNode);
}