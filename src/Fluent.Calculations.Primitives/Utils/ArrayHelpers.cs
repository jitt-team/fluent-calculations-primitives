namespace Fluent.Calculations.Primitives.Utils;

internal  static class ArrayHelpers
{
    public static T[] Concat<T>( T value, params T[] otherValues) => new T[] { value }.Concat(otherValues ?? []).ToArray();
}
