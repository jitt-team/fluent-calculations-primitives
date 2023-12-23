﻿namespace Fluent.Calculations.Primitives.BaseTypes;

/// <include file="Docs/IntelliSense.xml" path='docs/members[@name="IValueProvider"]/class/*' />
public interface IValueProvider : IValue
{
    IValueProvider MakeOfThisType(MakeValueArgs args);

    IValueProvider MakeDefault();

    IValue Accept(ValueVisitor visitor);
}
