using DotNetGraph.Core;
using DotNetGraph.Extensions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
namespace Fluent.Calculations.DotNetGraph;

public class CalculationDotGraphRenderer
{
    private IDotNetGraphBuilder builder;

    public CalculationDotGraphRenderer(IDotNetGraphBuilder builder) => this.builder = builder;

    public CalculationDotGraphRenderer() : this(new DotNetGraphBuilderDefault()) { }

    public DotGraph Render(IValue value)
    {
        DotGraph graph = builder.CreateDirectedGraph("FluentCalculations");
        DotSubgraph parametersCluster = builder.CreateInputParametersCluster();
        graph.Add(parametersCluster);
        AddValueToGraph(value, graph, parametersCluster);
        return graph;
    }

    private DotNodeBlock AddValueToGraph(IValue value, DotGraph targetGraph, DotSubgraph targerParametersCluster)
    {
        DotNodeBlock valueBlock = AddNewBlockByType(value);

        foreach (IValue childValue in value.Expression.Arguments)
        {
            DotNodeBlock argumentBlock = AddValueToGraph(childValue, targetGraph, targerParametersCluster);
            targetGraph.LinkValueToArgument(valueBlock, argumentBlock);
        }

        DotNodeBlock AddNewBlockByType(IValue value)
        {
            DotNodeBlock newBlock = CreateBlockByType(value);

            if (value.IsParameter)
                targerParametersCluster.Add(newBlock.LastNode);
            else
            {
                if (newBlock.IsValuePart)
                    targetGraph.Add(newBlock.LastNode);
                else
                    targetGraph.AddRange(newBlock.FirstNode, newBlock.ConnectorEdge, newBlock.LastNode);
            }

            return newBlock;
        }

        return valueBlock;
    }

    private DotNodeBlock CreateBlockByType(IValue value)
    {
        switch (value.Expression.Type)
        {
            case ExpressionNodeType.Lambda:
            case ExpressionNodeType.BinaryExpression:
                return CreateExpressionBlock(value);
            case ExpressionNodeType.None:
            case ExpressionNodeType.Constant:
            default:
                return CreateValueBlock(value);
        }
    }

    private DotNodeBlock CreateValueBlock(IValue value)
    {
        DotNode
            sameNode = builder.CreateConsantNode(value);

        return new DotNodeBlock
        {
            FirstNode = sameNode,
            LastNode = sameNode,
            IsValuePart = true
        };
    }
    private DotNodeBlock CreateExpressionBlock(IValue value)
    {
        DotNode
            firstNode = builder.CreateExpressionNode(value),
            lastNode = builder.CreateValueNode(value);

        return new DotNodeBlock
        {
            FirstNode = firstNode,
            ConnectorEdge = new DotEdge().From(firstNode).To(lastNode).WithPenWidth(2),
            LastNode = lastNode
        };
    }
}
