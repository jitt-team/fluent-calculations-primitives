using DotNetGraph.Core;
using Fluent.Calculations.DotNetGraph.Shared;
using Fluent.Calculations.Primitives.BaseTypes;
namespace Fluent.Calculations.DotNetGraph.Styles;

public interface IGraphStyle
{
    DotSubgraph CreateParametersCluster(string scope, int index);

    DotSubgraph CreateScopeCluster(string scope, int index);
    
    DotNodeBlock CreateBlock(IValue value);

    DotEdge ConnectValues(DotNode firstNode, DotNode lastNode);

    DotNode CreateFinalResult(IValue value);
}