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
}