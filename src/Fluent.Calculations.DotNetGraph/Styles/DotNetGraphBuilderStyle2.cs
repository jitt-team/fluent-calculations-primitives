using DotNetGraph.Core;
using Fluent.Calculations.DotNetGraph.Shared;
using Fluent.Calculations.Primitives.BaseTypes;

namespace Fluent.Calculations.DotNetGraph.Styles;

internal class DotNetGraphBuilderStyle2 : IGraphStyle
{
    public DotEdge ConnectValues(DotNode firstNode, DotNode lastNode)
    {
        throw new NotImplementedException();
    }

    public DotNodeBlock CreateBlock(IValue value)
    {
        throw new NotImplementedException();
    }

    public DotSubgraph CreateParametersCluster()
    {
        throw new NotImplementedException();
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
