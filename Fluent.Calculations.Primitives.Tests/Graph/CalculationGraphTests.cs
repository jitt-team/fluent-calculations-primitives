using DotNetGraph.Compilation;
using DotNetGraph.Core;
using DotNetGraph.Extensions;

namespace Fluent.Calculations.Primitives.Tests.Graph
{
    public class CalculationGraphTests
    {
        [Fact]
        public async Task Test()
        {
            DotGraph directedGraph = new DotGraph().WithIdentifier("FooGraph").Directed();
            DotNode fooNode = new DotNode()
                .WithIdentifier("FooBar")
                .WithShape(DotNodeShape.Rectangle)
                .WithLabel(ToHtmlNode(
                        "MyTestFinalResultNumber",
                        "FirstTestNumber + MyTestResultNumber",
                        "7.0"), isHtml: true);

            directedGraph.Add(fooNode);

            await SaveToDot(directedGraph);
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
            File.WriteAllText("graph.dot", result);


        }
    }
}