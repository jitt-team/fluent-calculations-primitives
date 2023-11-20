using DotNetGraph.Core;
using DotNetGraph.Extensions;
using Fluent.Calculations.DotNetGraph.Shared;
using Fluent.Calculations.DotNetGraph.Styles;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
namespace Fluent.Calculations.DotNetGraph;

public class DotGraphValueBuilder
{
    private readonly IDotNetGraphBuilder builder;

    public DotGraphValueBuilder(IDotNetGraphBuilder builder) => this.builder = builder;

    public DotGraphValueBuilder() : this(new DotNetGraphBuilderStyle1()) { }

    public DotGraph Build(IValue value)
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

            if (value.Origin == ValueOriginType.Parameter || value.Origin == ValueOriginType.Constant)
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
            expressionNode = builder.CreateExpressionNode(value),
            resultNode = builder.CreateValueNode(value);

        DotEdge connectorEdge = builder.CreateSolidEdge(expressionNode, resultNode);

        return new DotNodeBlock(resultNode, expressionNode, connectorEdge);
    }
}
