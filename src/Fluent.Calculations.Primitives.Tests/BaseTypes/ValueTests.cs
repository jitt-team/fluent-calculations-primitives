﻿namespace Fluent.Calculations.Primitives.Tests.BaseTypes;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using FluentAssertions;

public class ValueTests
{
    [Fact]
    public void ValueConstructor_WithArgs_CreatesExpectedInstance()
    {
        string expectedValueName = "TEST-VALUE-NAME";
        ExpressionNode expectedExpressionNode = new ExpressionNode("TEST-BODY", "TEST-TYPE");
        decimal expectedPrimitiveValue = 10m;

        FakeInheritedValue fakeValue = new(MakeValueArgs.Compose(expectedValueName, expectedExpressionNode, expectedPrimitiveValue));

        fakeValue.Name.Should().Be(expectedValueName);
        fakeValue.Primitive.Should().Be(expectedPrimitiveValue);
        fakeValue.Expression.Body.Should().Be(expectedExpressionNode.Body);
        fakeValue.Expression.Type.Should().Be(expectedExpressionNode.Type);
    }
}

public class FakeInheritedValue : Value
{
    public FakeInheritedValue(MakeValueArgs args) : base(args) { }
    public override IValue GetDefault() => new FakeInheritedValue(MakeValueArgs.Compose(StringConstants.NaN, ExpressionNode.None, 0));
    public override IValue MakeOfThisType(MakeValueArgs args) => new FakeInheritedValue(args);
}
