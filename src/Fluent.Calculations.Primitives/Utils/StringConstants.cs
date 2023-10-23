namespace Fluent.Calculations.Primitives;

internal static class StringConstants
{
    public const string NaN = "NaN";
    public const string Zero = "Zero";
    public const string Empty = "Empty";
    public const string ZeroDigit = "0";

    public static bool IsNaNOrNull(this string value) => string.IsNullOrEmpty(value) || value.Equals(NaN);
}
