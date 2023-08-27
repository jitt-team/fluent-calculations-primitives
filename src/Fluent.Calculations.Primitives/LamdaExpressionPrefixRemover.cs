namespace Fluent.Calculations.Primitives;

internal static class LamdaExpressionPrefixRemover
{
    public static string RemovePrefix(string body) => body.Replace("() => ", "");
}
