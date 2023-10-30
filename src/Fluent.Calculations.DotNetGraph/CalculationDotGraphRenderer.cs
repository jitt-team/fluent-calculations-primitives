using DotNetGraph.Core;
using DotNetGraph.Extensions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;

namespace Fluent.Calculations.DotNetGraph;

public class CalculationDotGraphRenderer
{
    private readonly IDotNetGraphBuilder builder;

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
            DotEdge edge = builder.CreateDashedEdge(valueBlock.LastNode, argumentBlock.FirstNode);
            targetGraph.Add(edge);
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
            case ExpressionNodeType.Collection:
            case ExpressionNodeType.MathExpression:
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
            constantNode = builder.CreateConsantNode(value);

        return new DotNodeBlock(constantNode, isValuePart: true);
    }
    private DotNodeBlock CreateExpressionBlock(IValue value)
    {
        DotNode
            firstNode = builder.CreateExpressionNode(value),
            lastNode = builder.CreateValueNode(value);

        DotEdge connectorEdge = builder.CreateSolidEdge(firstNode, lastNode);

        return new DotNodeBlock(firstNode, lastNode, connectorEdge);
    }
}
