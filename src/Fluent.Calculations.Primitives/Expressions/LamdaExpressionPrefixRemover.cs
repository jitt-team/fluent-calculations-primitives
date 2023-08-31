namespace Fluent.Calculations.Primitives.Expressions;

internal static class LamdaExpressionPrefixRemover
{
    public static string RemovePrefix(string body) => body.Replace("() => ", "");
}
