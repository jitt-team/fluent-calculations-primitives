using DotNetGraph.Compilation;
using DotNetGraph.Core;
using DotNetGraph.Extensions;
using FluentAssertions;
using Newtonsoft.Json.Linq;

namespace Fluent.Calculations.Primitives.Tests.Graph
{
    public class CalculationGraphTests
    {
        [Fact]
        public async Task Test()
        {
            Number result = new FooBarCalculation
            {
                ChildDeduction = Number.Of(200),
                NumberOfChildren = Number.Of(3),
                MinimumNumberOfChildren = Number.Of(2),
                GrossSalary = Number.Of(1000)
            }
            .Calculate();

            await RenderGraph(result);
        }

        private async Task RenderGraph(IValue value)
        {
            DotGraph graph = new DotGraph().WithIdentifier("FooGraph").Directed();
            ToNode(value, graph);
            await SaveToDot(graph);
        }

        private DotNode ToNode(IValue value, DotGraph graph)
        {
            var parent = new DotNode()
                 .WithIdentifier(value.Name)
                 .WithShape(DotNodeShape.Rectangle)
                 .WithLabel(ToHtmlNode(
                         $"{value.Name}",
                         System.Web.HttpUtility.HtmlEncode($"{value.Expresion.Body}"),
                         $"{value.ToString()}"),
                         isHtml: true);

            graph.Add(parent);
            foreach (IValue item in value.Expresion.Arguments)
            {
                var child = ToNode(item, graph);
                var edge = new DotEdge().From(child).To(parent);
                graph.Add(child);
                graph.Add(edge);
            }

            return parent;
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

        internal class FooBarCalculation : Calculation<Number>
        {
            public Number
                ChildDeduction = Number.Zero,
                NumberOfChildren = Number.Zero,
                MinimumNumberOfChildren = Number.Zero,
                GrossSalary = Number.Zero;

            Condition HasEnoughChildren => Is(() => NumberOfChildren > MinimumNumberOfChildren);

            Number TotalChildrenDeduction => Is(() => ChildDeduction * NumberOfChildren);

            Number AppliedChildDeduction => Is(() => HasEnoughChildren ? TotalChildrenDeduction : Number.Zero);

            Number TaxableSalary => Is(() => GrossSalary - AppliedChildDeduction);

            public override Number Return() => TaxableSalary;
        }
    }
}