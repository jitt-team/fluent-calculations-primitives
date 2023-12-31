﻿namespace Fluent.Calculations.Primitives.BaseTypes;
using System.Collections;
using System.Diagnostics;

/// <include file="Docs.xml" path='*/ArgumentsCollection/class/*'/>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(ArgumentsDebugView))]
public sealed class ArgumentsCollection : IArguments
{
    private readonly List<IValue> items;

    private ArgumentsCollection() => items = [];

    internal ArgumentsCollection(IEnumerable<IValue> arguments) => items = new List<IValue>(arguments);

    internal static ArgumentsCollection Empty => new();

    /// <include file="Docs.xml" path='*/ArgumentsCollection/Count/*'/>
    public int Count => items.Count;

    internal static ArgumentsCollection CreateFrom(IValue[] arguments) => new(arguments);

    /// <include file="Docs.xml" path='*/ArgumentsCollection/GetEnumerator/*'/>
    public IEnumerator<IValue> GetEnumerator() => items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();

    internal void AddRange(IArguments arguments) => items.AddRange(arguments);

    internal void Add(IValue arguments) => items.Add(arguments);
}