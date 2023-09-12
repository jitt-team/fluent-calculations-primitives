namespace Fluent.Calculations.Primitives.Expressions
{
    internal class BinaryExpressionLanguageConverter
    {
        public static string MethodNameToOperator(string methodName)
        {
            if (MethodOperatorMaping.TryGetValue(methodName, out string? @operator) && @operator != null)
                return @operator;

            return
                "#unknown_operator#";
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
