namespace Fluent.Calculations.Finance;

public class Currency : IComparable, IEquatable<Currency>, IComparable<Currency>
{
    public static Currency None => new Currency("None");

    // TODO: We could support alternate currency codes all at once to have highly compatible comparisions
    public string Code { get; }

    public Currency(string currencyCode) => Code = EnsureIsValid(currencyCode);

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

        if (currencyCode.Length != 3)
            throw new ArgumentException("", nameof(currencyCode));

        return currencyCode;
    }
}