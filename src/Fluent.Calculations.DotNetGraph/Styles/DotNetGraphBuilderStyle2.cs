using DotNetGraph.Attributes;
using DotNetGraph.Core;
using DotNetGraph.Extensions;
using Fluent.Calculations.DotNetGraph.Shared;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;

namespace Fluent.Calculations.DotNetGraph.Styles;

internal class DotNetGraphBuilderStyle2 : IGraphStyle
{
    public DotEdge ConnectValues(DotNode firstNode, DotNode lastNode) =>
        new DotEdge().From(lastNode).To(firstNode)
                .WithStyle(DotEdgeStyle.Dashed)
                .WithArrowHead(DotEdgeArrowType.Open);

    public DotNodeBlock CreateBlock(IValue value)
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

    public DotSubgraph CreateParametersCluster()
    {
        DotSubgraph subgraph = new DotSubgraph()
            .WithIdentifier("cluster_0")
            .WithLabel("INPUT PARAMETERS")
            .WithColor(DotColor.Black)
            .WithStyle("filled, solid");

        // subgraph.SetAttribute("fillcolor", new DotAttribute($@"""#c27ffa"""));

        return subgraph;
    }

    /*
        style = "filled"
        penwidth = 1 
        fillcolor = "white"
        fontname = "Courier New"
        shape = "Mrecord"
    */

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
