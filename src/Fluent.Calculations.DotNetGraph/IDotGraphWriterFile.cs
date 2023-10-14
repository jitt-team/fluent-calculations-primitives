using DotNetGraph.Core;

namespace Fluent.Calculations.DotNetGraph
{
    internal interface IDotGraphWriterFile
    {
        Task SaveToDot(DotGraph graph, string outputFilePath);
    }
}