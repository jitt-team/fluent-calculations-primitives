using DotNetGraph.Compilation;
using DotNetGraph.Core;
using DotNetGraph.Extensions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;

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
        ToNode(value, graph);
        await SaveToDot(graph);
    }

    private DotNode ToNode(IValue value, DotGraph graph)
    {
        var parent = ComposeDotNodeByType(value);
        graph.Add(parent);

        foreach (IValue item in value.Expression.Arguments)
        {
            var child = ToNode(item, graph);
            var edge = new DotEdge().From(child).To(parent);
            graph.Add(child);
            graph.Add(edge);
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
              .WithShape(value.IsOutput ? DotNodeShape.MSquare : 
                                value.IsInput ? DotNodeShape.MDiamond : 
                                        DotNodeShape.Rectangle)
              .WithLabel(ToHtmlNode(
                      System.Web.HttpUtility.HtmlEncode($"{value.Name}"),
                      System.Web.HttpUtility.HtmlEncode($"{value.Expression.Body}"),
                      System.Web.HttpUtility.HtmlEncode($"{value.Primitive:0.00}")),
                      isHtml: true);
    }

    private string ToHtmlNode(string name, string expression, string value)
    {
        return $@"<table border=""0"">
                    <tr><td align=""left""><b>{name}</b></td></tr>
                    <tr><td align=""left"">{expression}</td></tr>
                    <tr><td align=""left"">{value}</td></tr>
                </table>";

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
