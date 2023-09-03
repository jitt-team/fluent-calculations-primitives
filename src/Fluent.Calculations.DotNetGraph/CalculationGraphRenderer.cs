using DotNetGraph.Compilation;
using DotNetGraph.Core;
using DotNetGraph.Extensions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Linq.Expressions;
using System.Web;

namespace Fluent.Calculations.DotNetGraph;

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
            .WithLabel("Input Parameters")
            .WithColor(DotColor.LightGray)
            .WithStyle(DotSubgraphStyle.Filled);

        graph.Add(inputsCluster);
        ToNode(value, graph, inputsCluster);
        await SaveToDot(graph);
    }

    private DotNode ToNode(IValue value, DotGraph graph, DotSubgraph inputsCluster)
    {
        DotNode parent = ComposeDotNodeByType(value);
        AddNode(parent, value);

        foreach (IValue childValue in value.Expression.Arguments)
        {
            DotNode child = ToNode(childValue, graph, inputsCluster);
            DotEdge edge = new DotEdge().From(child).To(parent);
            graph.Add(edge);
        }

        void AddNode(DotNode node, IValue value)
        {
            if (value.IsInput)
                inputsCluster.Add(node);
            else
                graph.Add(node);
        }

        return parent;
    }

    private DotNode ComposeDotNodeByType(IValue value)
    {
        switch (value.Expression.Type)
        {
            case ExpressionNodeType.None:
                break;
            case ExpressionNodeType.Comparision:
                break;
            case ExpressionNodeType.Conditional:
                break;
            case ExpressionNodeType.Constant:
                break;
            case ExpressionNodeType.BinaryExpression:
                break;
            default:
                return ComposeDotNodeDefault(value);
        }

        return ComposeDotNodeDefault(value);
    }

    private DotNode ComposeDotNodeDefault(IValue value)
    {
        return new DotNode()
              .WithIdentifier(System.Web.HttpUtility.HtmlEncode(value.Name))
              .WithShape(ShapyByValueType())
              .WithLabel(ToHtmlNode(value), isHtml: true);

        string ShapyByValueType() => value.IsOutput ? "ellipse" :
                                value.IsInput ? "parallelogram" :
                                        "Rectangle";
    }

    private string ToHtmlNode(IValue value)
    {
        return $@"<table border=""0"">
                    <tr><td align=""left""><b>{Html(value.Name)}</b></td></tr>
                    {ExpressionBlockIfRequired()}
                    <tr><td align=""left"">{value.Primitive:0.00}</td></tr>
                </table>";

        string ExpressionBlockIfRequired() => value.IsInput ? string.Empty : InputExpressionBlock();
        string InputExpressionBlock() => $@"<tr><td align=""left"">{Html(value.Expression.Body)}</td></tr>";
        string Html(string value) => HttpUtility.HtmlEncode(value);
    }

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
