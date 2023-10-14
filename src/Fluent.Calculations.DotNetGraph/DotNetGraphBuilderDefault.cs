using DotNetGraph.Attributes;
using DotNetGraph.Core;
using DotNetGraph.Extensions;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Web;

namespace Fluent.Calculations.DotNetGraph
{
    public class DotNetGraphBuilderDefault : IDotNetGraphBuilder
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
                  .WithShape("Rectangle")
                  .WithFillColor("skyblue")
                  .WithStyle(DotNodeStyle.Filled)
                  .WithLabel(ToExpressionNodeHtml(value), isHtml: true);

            node.SetAttribute("margin", new DotAttribute(@"""0.07"""));

            return node;
        }

        private string ShapyByValueType(IValue value)
        {
            if (value.IsOutput)
                return "ellipse";

            if (value.IsParameter)
                return "parallelogram";

            return "Rectangle";
        }

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
                    <tr><td align=""center"">{value.ValueToString()}</td></tr>
                </table>";
        }
        private string ToValueNodeHtml(IValue value)
        {
            return $@"<table border=""0"">
                    <tr><td align=""center""><b>{Html(value.Name)}</b></td></tr>
                    <tr><td align=""center"">{value.ValueToString()}</td></tr>
                </table>";
        }


        string Html(string value) => HttpUtility.HtmlEncode(value);

    }


}
