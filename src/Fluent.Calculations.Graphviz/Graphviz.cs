using System.Diagnostics;
using System.Reflection;

namespace Fluent.Calculations.Graphviz
{
    internal class Graphviz
    {
        public void ConvertToPNG(string dotFilePath)
        {
            string? applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (applicationPath == null)
                return;

            Process proc = new();
            proc.StartInfo.FileName = Path.Combine(applicationPath, @"graphviz\bin\dot.exe");
            proc.StartInfo.Arguments = $"-T png -O {dotFilePath} -Gdpi=150";
            proc.Start();
            proc.WaitForExit();
        }
    }
}
