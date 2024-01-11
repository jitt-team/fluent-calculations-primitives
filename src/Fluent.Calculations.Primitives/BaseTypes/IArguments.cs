﻿namespace Fluent.Calculations.Primitives.BaseTypes;

/// <include file="Docs.xml" path='*/IArguments/inteface/*'/>
public interface IArguments : IEnumerable<IValue>
{
    /// <include file="Docs.xml" path='*/IArguments/Count/*'/>
    public int Count { get; }
}