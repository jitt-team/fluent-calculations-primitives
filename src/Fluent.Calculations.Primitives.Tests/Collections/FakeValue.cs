﻿using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;

namespace Fluent.Calculations.Primitives.Tests.Collections
{
    internal class FakeValue : IValue
    {
        public string Name { get; set; }

        public decimal Primitive { get; set; }

        public bool IsParameter { get; set; }

        public bool IsOutput { get; set; }

        public ExpressionNode Expression { get; set; }

        public TagsCollection Tags { get; set; }

        public IValue Default => new FakeValue();

        public IValue MakeOfThisType(MakeValueArgs args) => new FakeValue
        {
            Name = args.Name
        };

        public string ValueToString() => "VALUE-AS-STRING";
    }
}