namespace Fluent.Calculations.Primitives.Tests.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using FluentAssertions;

public class BinaryOperatorHandlerHandlerTests
{
    [Fact]
    public void BinaryExpression_WithTwoValues_IsExpectedResult()
    {
        Number
            leftNumber = Number.Of(2.00m, "LEFT-NUMBER"),
            rightNumber = Number.Of(3.00m, "RIGHT-NUMBER");

        Number result = BinaryOperatorHandler.Handle<Number, decimal>(leftNumber, rightNumber, (a, b) => a.Primitive + b.Primitive, "Add", ExpressionNodeType.BinaryExpression);

        result.Primitive.Should().Be(5.00m);
        result.Name.Should().Be("Add");
        result.Expression.Arguments.Should().HaveCount(2);
        result.Expression.Arguments.Should().ContainSingle(a => a.Name.Equals("LEFT-NUMBER"));
        result.Expression.Arguments.Should().ContainSingle(a => a.Name.Equals("RIGHT-NUMBER"));
        result.Expression.Body.Should().Be("LEFT-NUMBER + RIGHT-NUMBER");
    }
}
