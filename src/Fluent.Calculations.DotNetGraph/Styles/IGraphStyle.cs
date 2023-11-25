using DotNetGraph.Core;
using Fluent.Calculations.DotNetGraph.Shared;
using Fluent.Calculations.Primitives.BaseTypes;
namespace Fluent.Calculations.DotNetGraph.Styles;

public interface IGraphStyle
{
    DotSubgraph CreateParametersCluster();

    DotNodeBlock CreateBlock(IValue value);

    DotEdge ConnectValues(DotNode firstNode, DotNode lastNode);
}