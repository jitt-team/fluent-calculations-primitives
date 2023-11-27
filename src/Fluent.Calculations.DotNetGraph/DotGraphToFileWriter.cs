using DotNetGraph.Compilation;
using DotNetGraph.Core;
namespace Fluent.Calculations.DotNetGraph;

public class DotGraphToFileWriter : IDotGraphToFileWriter
{
    public async Task SaveToDot(DotGraph graph, string outputFilePath)
    {
        await using var writer = new StringWriter();
        CompilationContext context = new(writer, new CompilationOptions());
        await graph.CompileAsync(context);
        string result = writer.GetStringBuilder().ToString();
        File.WriteAllText(outputFilePath, result);
    }
}
