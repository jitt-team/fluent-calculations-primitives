namespace Fluent.Calculations.Primitives.Tests.ComplexValueType;

public class Currency(string currencyCode) : IComparable, IEquatable<Currency>, IComparable<Currency>
{
    private const string NoneCorrencyCode = "None";

    public static Currency None => new(NoneCorrencyCode);

    public string Code { get; } = EnsureIsValid(currencyCode);

    public override string ToString() => $"{Code}";

    public int CompareTo(object? obj) => Code.CompareTo(obj as Currency);

    public bool Equals(Currency? other) => Code.Equals(other?.Code);

    public int CompareTo(Currency? other) => Code.CompareTo(other?.Code);

    public override bool Equals(object? obj) => Equals(obj as Currency);

    public override int GetHashCode() => Code.GetHashCode();

    public static bool operator ==(Currency left, Currency right) => Equals(left, right);

    public static bool operator !=(Currency left, Currency right) => !Equals(left, right);

    private static string EnsureIsValid(string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
            throw new ArgumentNullException(nameof(currencyCode));

        if (currencyCode.Equals(NoneCorrencyCode))
            return currencyCode;

        if (currencyCode.Length != 3)
            throw new ArgumentException("", nameof(currencyCode));

        return currencyCode;
    }
}