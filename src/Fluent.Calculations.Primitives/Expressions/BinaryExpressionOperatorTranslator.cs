namespace Fluent.Calculations.Primitives.Expressions
{
    internal class BinaryExpressionOperatorTranslator
    {
        public const string Unknown = "<Unknown>";

        public static string MethodNameToOperator(string methodName)
        {
            if (MethodOperatorMaping.TryGetValue(methodName, out string? @operator) && @operator != null)
                return @operator;

            return Unknown;
        }

        private static Dictionary<string, string> MethodOperatorMaping = new Dictionary<string, string>
        {
            ["And"] = "&",
            ["Or"] = "|",
            ["IsEqual"] = "==",
            ["NotEqual"] = "!=",
            ["LessThan"] = "<",
            ["GreaterThan"] = ">",
            ["LessThanOrEqual"] = "<=",
            ["GreaterThanOrEqual"] = ">=",
            ["Add"] = "+",
            ["Substract"] = "-",
            ["Multiply"] = "*",
            ["Divide"] = "/"
        };
    }
}
