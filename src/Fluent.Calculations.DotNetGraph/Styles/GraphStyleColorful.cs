using DotNetGraph.Attributes;
using DotNetGraph.Core;
using DotNetGraph.Extensions;
using Fluent.Calculations.DotNetGraph.Shared;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Web;
namespace Fluent.Calculations.DotNetGraph.Styles;

public class GraphStyleColorful : IGraphStyle
{
    public DotNodeBlock CreateBlock(IValue value)
    {
        return value.Expression.Type switch
        {
            ExpressionNodeType.Lambda or
            ExpressionNodeType.BinaryExpression or
            ExpressionNodeType.Collection or
            ExpressionNodeType.MathExpression
            => CreateExpressionBlock(value),
            _ => CreateValueBlock(value),
        };
    }
    public DotSubgraph CreateParametersCluster(string scope, int index)
    {
        DotSubgraph subgraph = new DotSubgraph()
            .WithIdentifier($"cluster_{index}")
            .WithLabel($"{scope.ToUpper()} PARAMETERS")
            .WithColor(DotColor.Black)
            .WithStyle("filled, solid");

        subgraph.SetAttribute("fillcolor", new DotAttribute($@"""#c27ffa"""));

        return subgraph;
    }

    private static DotNodeBlock CreateValueBlock(IValue value)
    {
        DotNode
            constantNode = CreateConsantNode(value);

        return new DotNodeBlock(constantNode, isValuePart: true);
    }

    private static DotNodeBlock CreateExpressionBlock(IValue value)
    {
        DotNode
            expressionNode = CreateExpressionNode(value),
            resultNode = CreateValueNode(value);

        DotEdge connectorEdge = ConnectTwoSubNodes(expressionNode, resultNode);

        return new DotNodeBlock(resultNode, expressionNode, connectorEdge);
    }

    private static DotNode CreateConsantNode(IValue value)
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

    private static DotNode CreateValueNode(IValue value)
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

    private static DotNode CreateExpressionNode(IValue value)
    {
        var node = new DotNode()
              .WithIdentifier(Html($"{value.Name}_expression"))
              .WithShape("rectangle")
              .WithFillColor("skyblue")
              .WithStyle(DotNodeStyle.Filled)
              .WithLabel(ToExpressionNodeHtml(value), isHtml: true);

        node.SetAttribute("margin", new DotAttribute(@"""0.07"""));

        return node;
    }

    private static string ShapyByValueType(IValue value)
    {
        if (value.Origin == ValueOriginType.Result)
            return "ellipse";

        if (value.Origin == ValueOriginType.Parameter)
            return "parallelogram";

        return "Rectangle";
    }

    private static string ColorByValueType(IValue value) => value.Origin == ValueOriginType.Result ?
            "#7ffac2" : "skyblue";

    private static string ToExpressionNodeHtml(IValue value)
    {
        return $@"<table border=""0"">
                    <tr><td align=""center""><b>{Html(value.Name)}</b></td></tr>
                    <tr><td align=""left"">{Html(value.Expression.Body)}</td></tr>
                </table>";
    }

    private static string ToConstantNodeHtml(IValue value)
    {
        return $@"<table border=""0"">
                    <tr><td align=""left""><b>{Html(value.Name)}</b></td></tr>
                    <tr><td align=""center"">{value.PrimitiveString}</td></tr>
                </table>";
    }
    private static string ToValueNodeHtml(IValue value)
    {
        return $@"<table border=""0"">
                    <tr><td align=""center""><b>{Html(value.Name)}</b></td></tr>
                    <tr><td align=""center"">{value.PrimitiveString}</td></tr>
                </table>";
    }

    private static string Html(string value) => HttpUtility.HtmlEncode(value);

    private static DotEdge ConnectTwoSubNodes(DotNode firstNode, DotNode lastNode) =>
        new DotEdge().From(firstNode).To(lastNode).WithPenWidth(2);

    public DotEdge ConnectValues(DotNode firstNode, DotNode lastNode) =>
        new DotEdge().From(lastNode).To(firstNode)
                .WithStyle(DotEdgeStyle.Dashed)
                .WithArrowHead(DotEdgeArrowType.Open);
}
