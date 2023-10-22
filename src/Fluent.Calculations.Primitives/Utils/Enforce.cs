﻿using System.Runtime.CompilerServices;

namespace Fluent.Calculations.Primitives.Utils;

internal static class Enforce
{
    public static T NotNull<T>(T? condition, [CallerMemberName] string argumentName = StringConstants.NaN) => condition ?? throw new ArgumentNullException(argumentName);
}
