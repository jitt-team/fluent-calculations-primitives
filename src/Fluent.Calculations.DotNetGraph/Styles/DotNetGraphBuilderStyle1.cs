using DotNetGraph.Attributes;
using DotNetGraph.Core;
using DotNetGraph.Extensions;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Web;

namespace Fluent.Calculations.DotNetGraph.Styles
{
    public class DotNetGraphBuilderStyle1 : IDotNetGraphBuilder
    {
        public DotGraph CreateDirectedGraph(string identifier) =>
            new DotGraph().WithIdentifier(identifier).Directed();

        public DotSubgraph CreateInputParametersCluster()
        {
            DotSubgraph subgraph = new DotSubgraph()
                .WithIdentifier("cluster_0")
                .WithLabel("INPUT PARAMETERS")
                .WithColor(DotColor.Black)
                .WithStyle("filled, solid");

            subgraph.SetAttribute("fillcolor", new DotAttribute($@"""#c27ffa"""));

            return subgraph;
        }

        public DotNode CreateConsantNode(IValue value)
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

        public DotNode CreateValueNode(IValue value)
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

        public DotNode CreateExpressionNode(IValue value)
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
                    <tr><td align=""center"">{value.ValueToString()}</td></tr>
                </table>";
        }
        private static string ToValueNodeHtml(IValue value)
        {
            return $@"<table border=""0"">
                    <tr><td align=""center""><b>{Html(value.Name)}</b></td></tr>
                    <tr><td align=""center"">{value.ValueToString()}</td></tr>
                </table>";
        }

        private static string Html(string value) => HttpUtility.HtmlEncode(value);

        public DotEdge CreateSolidEdge(DotNode firstNode, DotNode lastNode) =>
            new DotEdge().From(firstNode).To(lastNode).WithPenWidth(2);

        public DotEdge CreateDashedEdge(DotNode firstNode, DotNode lastNode) =>
            new DotEdge().From(lastNode).To(firstNode)
                    .WithStyle(DotEdgeStyle.Dashed)
                    .WithArrowHead(DotEdgeArrowType.Open);

        private string ComposeHtmlLabel(string name, string expression, string value) => $@"
            <table border=""0"" cellborder=""0"" cellpadding=""3"" bgcolor=""white"">
                <tr>
                    <td bgcolor=""black"" align=""center"" colspan=""2""><font color=""white"">{name}</font></td>
                </tr>
                <tr>
                    <td align=""left"" port=""r1"">{expression}</td>
                    <td bgcolor=""grey"" align=""center"">{value}</td>
                </tr>
            </table>";
    }
}

/*
    style = "filled"
    penwidth = 1 
    fillcolor = "white"
    fontname = "Courier New"
    shape = "Mrecord"
*/