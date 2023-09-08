﻿using DotNetGraph.Attributes;
using DotNetGraph.Compilation;
using DotNetGraph.Core;
using DotNetGraph.Extensions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Web;

namespace Fluent.Calculations.DotNetGraph;

public class DotNodeBlock
{
    public bool IsValue { get; set; }
    public DotNode FirstNode { get; set; }
    public DotEdge ConnectorEdge { get; set; }
    public DotNode LastNode { get; set; }
}

public class CalculationGraphRenderer
{
    private readonly string graphFileName;

    public CalculationGraphRenderer(string graphFileName)
    {
        this.graphFileName = graphFileName;
    }

    public async Task Render(IValue value)
    {
        DotGraph graph = new DotGraph().WithIdentifier("FooGraph").Directed();
        DotSubgraph inputsCluster = new DotSubgraph()
            .WithIdentifier("cluster_0")
            .WithLabel("INPUT PARAMETERS")
            .WithColor(DotColor.Black)
            .WithStyle("filled, solid");

        inputsCluster.SetAttribute("fillcolor", new DotAttribute($@"""#c27ffa"""));

        graph.Add(inputsCluster);
        ToNode(value, graph, inputsCluster);
        await SaveToDot(graph);
    }

    private DotNodeBlock ToNode(IValue value, DotGraph graph, DotSubgraph inputsCluster)
    {
        DotNodeBlock parentNode = ComposeDotNodeByType(value);
        AddNode(parentNode, value);

        foreach (IValue childValue in value.Expression.Arguments)
        {
            DotNodeBlock child = ToNode(childValue, graph, inputsCluster);
            DotEdge edge = new DotEdge().From(child.LastNode).To(parentNode.FirstNode).WithStyle(DotEdgeStyle.Dashed).WithArrowHead(DotEdgeArrowType.Open);
            graph.Add(edge);
        }

        void AddNode(DotNodeBlock node, IValue value)
        {
            if (value.IsInput)
                inputsCluster.Add(node.LastNode);
            else
            {
                if (node.IsValue)
                    graph.Add(node.LastNode);
                else
                {
                    graph.Add(node.FirstNode);
                    graph.Add(node.ConnectorEdge);
                    graph.Add(node.LastNode);
                }
            }
        }

        return parentNode;
    }

    private DotNodeBlock ComposeDotNodeByType(IValue value)
    {
        switch (value.Expression.Type)
        {
            case ExpressionNodeType.Lambda:
            case ExpressionNodeType.BinaryExpression:
                return ToExpressionDotNodeBlock(value);
            case ExpressionNodeType.None:
            case ExpressionNodeType.Constant:
            default:
                return ToValueDotNodeBlock(value);
        }
    }

    private DotNodeBlock ToValueDotNodeBlock(IValue value)
    {
        DotNode
            sameNode = ToConstantDotNode(value);

        return new DotNodeBlock
        {
            FirstNode = sameNode,
            LastNode = sameNode,
            IsValue = true
        };
    }
    private DotNodeBlock ToExpressionDotNodeBlock(IValue value)
    {
        DotNode
            firstNode = ToExpressionDotNode(value),
            lastNode = ToValueDotNode(value);

        return new DotNodeBlock
        {
            FirstNode = firstNode,
            ConnectorEdge = new DotEdge().From(firstNode).To(lastNode).WithPenWidth(2),
            LastNode = lastNode
        };
    }

    private DotNode ToConstantDotNode(IValue value)
    {
        var node = new DotNode()
              .WithIdentifier(Html($"{value.Name}_value"))
              .WithShape(ShapyByValueType(value))
              .WithFillColor(ColorByValueType(value))
              .WithStyle(DotNodeStyle.Filled)
              .WithLabel(ToConstantNodeHtml(value), isHtml: true);

        node.SetAttribute("margin", new DotAttribute(@"""0.07"""));

        return node;
    }

    private DotNode ToValueDotNode(IValue value)
    {
        var node = new DotNode()
              .WithIdentifier(Html($"{value.Name}_value"))
              .WithShape(DotNodeShape.Ellipse)
              .WithFillColor(ColorByValueType(value))
              .WithStyle(DotNodeStyle.Filled)
              .WithLabel(ToValueNodeHtml(value), isHtml: true);

        node.SetAttribute("margin", new DotAttribute(@"""0.07"""));

        return node;
    }

    private DotNode ToExpressionDotNode(IValue value)
    {
        var node =  new DotNode()
              .WithIdentifier(Html($"{value.Name}_expression"))
              .WithShape("Rectangle")
              .WithFillColor("skyblue")
              .WithStyle(DotNodeStyle.Filled)
              .WithLabel(ToExpressionNodeHtml(value), isHtml: true);

        node.SetAttribute("margin", new DotAttribute(@"""0.07"""));

        return node;
    }

    private string ShapyByValueType(IValue value) =>
                value.IsOutput ? "ellipse" :
                value.IsInput ? "parallelogram" :
                                        "Rectangle";

    private string ColorByValueType(IValue value) => value.IsOutput ?
            "#7ffac2" : "skyblue";

    private string ToExpressionNodeHtml(IValue value)
    {
        return $@"<table border=""0"">
                    <tr><td align=""center""><b>{Html(value.Name)}</b></td></tr>
                    <tr><td align=""left"">{Html(value.Expression.Body)}</td></tr>
                </table>";
    }

    private string ToConstantNodeHtml(IValue value)
    {
        return $@"<table border=""0"">
                    <tr><td align=""left""><b>{Html(value.Name)}</b></td></tr>
                    <tr><td align=""center"">{value.Primitive}</td></tr>
                </table>";
    }
    private string ToValueNodeHtml(IValue value)
    {
        return $@"<table border=""0"">
                    <tr><td align=""center""><b>{Html(value.Name)}</b></td></tr>
                    <tr><td align=""center"">{value.Primitive}</td></tr>
                </table>";
    }

    string Html(string value) => HttpUtility.HtmlEncode(value);

    private async Task SaveToDot(DotGraph graph)
    {
        await using var writer = new StringWriter();
        var context = new CompilationContext(writer, new CompilationOptions());
        await graph.CompileAsync(context);

        var result = writer.GetStringBuilder().ToString();

        // Save it to a file
        File.WriteAllText(graphFileName, result);
    }
}
