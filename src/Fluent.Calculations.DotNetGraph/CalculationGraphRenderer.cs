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
            .WithColor(DotColor.LightSkyBlue)
            .WithStyle(DotSubgraphStyle.Filled);

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
            DotEdge edge = new DotEdge().From(child.LastNode).To(parentNode.FirstNode);
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
            case ExpressionNodeType.Comparision:
            case ExpressionNodeType.Conditional:
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
            sameNode = ToValueDotNode(value);

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
            ConnectorEdge = new DotEdge().From(firstNode).To(lastNode),
            LastNode = lastNode
        };
    }

    private DotNode ToValueDotNode(IValue value)
    {
        return new DotNode()
              .WithIdentifier(Html($"{value.Name}_value"))
              .WithShape(ShapyByValueType(value))
              .WithFillColor(ColorByValueType(value))
              .WithStyle(DotNodeStyle.Filled)
              .WithLabel(ToValueNodeHtml(value), isHtml: true);
    }

    private DotNode ToExpressionDotNode(IValue value)
    {
        return new DotNode()
              .WithIdentifier(Html($"{value.Name}_expression"))
              .WithShape(ShapyByValueType(value))
              .WithFillColor(ColorByValueType(value))
              .WithStyle(DotNodeStyle.Filled)
              .WithLabel(ToExpressionNodeHtml(value), isHtml: true);
    }

    private string ShapyByValueType(IValue value) =>
                value.IsOutput ? "ellipse" :
                value.IsInput ? "parallelogram" :
                                        "Rectangle";

    private DotColor ColorByValueType(IValue value) => value.IsOutput ?
            DotColor.PaleGreen : DotColor.White;

    private string ToExpressionNodeHtml(IValue value)
    {
        return $@"<table border=""0"">
                    <tr><td align=""left"">{Html(value.Expression.Body)}</td></tr>
                </table>";
    }

    private string ToValueNodeHtml(IValue value)
    {
        return $@"<table border=""0"">
                    <tr><td align=""left""><b>{Html(value.Name)}</b></td></tr>
                    <tr><td align=""left"">{value.Primitive:0.00}</td></tr>
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
