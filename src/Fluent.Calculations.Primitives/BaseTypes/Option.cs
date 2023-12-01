﻿namespace Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Runtime.CompilerServices;

public static class Option
{
    public static Option<TEnum> Of<TEnum>(TEnum primitiveValue, [CallerMemberName] string fieldName = StringConstants.NaN) where TEnum : struct, Enum
        => new Option<TEnum>(MakeValueArgs.Compose(fieldName, new ExpressionNode($"{primitiveValue}", ExpressionNodeType.Constant), 
            Convert.ToDecimal((int)(object)primitiveValue)) /* explore if this cast can be optimized */);
}

public class Option<T> : Value where T : struct, Enum
{
    public Option() : this(MakeValueArgs.Compose(StringConstants.Zero, new ExpressionNode(StringConstants.ZeroDigit, ExpressionNodeType.Constant), 0)) { }

    public Option(Option<T> enumValue) : base(enumValue) { }

    public Option(MakeValueArgs makeValueArgs) : base(makeValueArgs) { }

    public override IValueProvider MakeDefault()
    {
        throw new NotImplementedException();
    }

    public override IValueProvider MakeOfThisType(MakeValueArgs args)
    {
        throw new NotImplementedException();
    }
}
