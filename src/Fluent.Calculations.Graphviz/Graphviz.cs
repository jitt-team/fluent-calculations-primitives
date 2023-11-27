using System.Diagnostics;
using System.Reflection;

namespace Fluent.Calculations.Graphviz
{
    internal static class Graphviz
    {
        public static void ConvertToPNG(string dotFilePath)
        {
            string? applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (applicationPath == null)
                return;

            Process proc = new();
            proc.StartInfo.FileName = Path.Combine(applicationPath, @"graphviz\bin\dot.exe");
            proc.StartInfo.Arguments = $"-T png -O {dotFilePath} -Gdpi=150  -Gsize=16,9";
            proc.Start();
            proc.WaitForExit();
        }
    }
}
