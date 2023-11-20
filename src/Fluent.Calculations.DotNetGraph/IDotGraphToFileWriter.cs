using DotNetGraph.Core;

namespace Fluent.Calculations.DotNetGraph
{
    internal interface IDotGraphToFileWriter
    {
        Task SaveToDot(DotGraph graph, string outputFilePath);
    }
}