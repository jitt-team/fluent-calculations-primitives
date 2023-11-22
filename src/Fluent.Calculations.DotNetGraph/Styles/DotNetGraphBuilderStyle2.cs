using DotNetGraph.Attributes;
using DotNetGraph.Core;
using DotNetGraph.Extensions;
using Fluent.Calculations.DotNetGraph.Shared;
using Fluent.Calculations.Primitives.BaseTypes;
using System.Text.RegularExpressions;
using System.Web;
namespace Fluent.Calculations.DotNetGraph.Styles;

internal class DotNetGraphBuilderStyle2 : IGraphStyle
{
    public DotSubgraph CreateParametersCluster()
    {
        DotSubgraph subgraph = new DotSubgraph()
            .WithIdentifier("cluster_0")
            .WithLabel("Parameters")
            .WithColor(DotColor.LightGrey)
            .WithStyle("filled, solid");

        subgraph.SetAttribute("penwidth", new DotAttribute(@"""1"""));
        subgraph.SetAttribute("fontname", new DotAttribute(@"""Courier New"""));

        return subgraph;
    }

    public DotEdge ConnectValues(DotNode firstNode, DotNode lastNode) =>
        new DotEdge().From(lastNode).To(firstNode)
                .WithStyle(DotEdgeStyle.Solid)
                .WithArrowHead(DotEdgeArrowType.Normal);

    public DotNodeBlock CreateBlock(IValue value) => CreateValueBlock(value);

    private DotNodeBlock CreateValueBlock(IValue value)
    {
        DotNode node = CreateBaseNodeStyle(value.Name).WithLabel(ComposeHtmlLabel(value), isHtml: true);
        return new DotNodeBlock(node, true);
    }

    private DotNode CreateBaseNodeStyle(string name)
    {
        DotNode node = new DotNode()
                .WithIdentifier(Html($"{name}_value"))
                .WithPenWidth(1)
                .WithFillColor(DotColor.White)
                .WithShape("Mrecord")
                .WithStyle(DotNodeStyle.Filled);

        node.SetAttribute("fontname", new DotAttribute(@"""Courier New"""));

        return node;
    }

    private string ComposeHtmlLabel(IValue value) => $@"
            <table border=""0"" cellborder=""0"" cellpadding=""3"" bgcolor=""white"">
                <tr>
                    <td bgcolor=""black"" align=""center"" colspan=""2""><font color=""white"">{Humanize(value.Name)}</font></td>
                </tr>
                <tr>
                    {ValueRow(value)}
                </tr>
            </table>";

    private string ValueRow(IValue value) => IsParameter(value) ?
            $@"
                <td align=""center"" port=""r1"">{Html(value.Expression.Body)}</td>" :
            $@"
                <td align=""left"" port=""r1"">{Html(Humanize(value.Expression.Body))} : </td>
                <td bgcolor=""grey"" align=""center"">{value.ValueToString()}</td>";

    private bool IsParameter(IValue value) => value.Origin == ValueOriginType.Constant || value.Origin == ValueOriginType.Parameter;

    private static string Html(string value) => HttpUtility.HtmlEncode(value);

    private string Humanize(string cammelCaseText) => Regex.Replace(cammelCaseText, "([A-Z])", " $1", RegexOptions.Compiled, TimeSpan.FromSeconds(1)).Trim();
}
