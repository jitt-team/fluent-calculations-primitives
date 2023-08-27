namespace Fluent.Calculations.Primitives;

public partial class Scope<TResult> where TResult : class, IValue, new()
{
    internal class LamdaExpressionPrefixRemover
    {
        public static string RemovePrefix(string body) => body.Replace("() => ", "");
    }
}