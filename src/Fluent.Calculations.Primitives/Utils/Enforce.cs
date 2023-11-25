using Fluent.Calculations.Primitives.BaseTypes;
using System.Runtime.CompilerServices;
namespace Fluent.Calculations.Primitives.Utils;

internal static class Enforce
{
    public static T NotNull<T>(T? condition, [CallerMemberName] string argumentName = StringConstants.NaN)  where T : IValueProvider => condition ?? throw new ArgumentNullException(argumentName);

    public static string IsNullOrWhiteSpace(string value) => !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentNullException(nameof(value));
}
