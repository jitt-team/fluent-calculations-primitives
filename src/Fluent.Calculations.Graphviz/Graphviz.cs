using System.Diagnostics;

namespace Fluent.Calculations.Graphviz
{
    internal class Graphviz
    {
        public void ConvertToPNG(string dotFilePath)
        {
            Process proc = new();
            proc.StartInfo.FileName = @"graphviz\bin\dot.exe";
            proc.StartInfo.Arguments = $"-T png -O {dotFilePath}";
            proc.Start();

        }
    }
}
