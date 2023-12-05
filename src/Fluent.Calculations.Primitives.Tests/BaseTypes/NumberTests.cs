namespace Fluent.Calculations.Primitives.Tests.BaseTypes;
using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

public class NumberTests
{
    [Fact]
    public void Add_TwoNumbers_ExpectedResult() => ExecuteMathExpressionAndAssertResult((left, right) => left + right, 6);

    [Fact]
    public void Substract_TwoNumbers_ExpectedResult() => ExecuteMathExpressionAndAssertResult((left, right) => left - right, 2);

    [Fact]
    public void Multiply_TwoNumbers_ExpectedResult() => ExecuteMathExpressionAndAssertResult((left, right) => left * right, 8);

    [Fact]
    public void Divide_TwoNumbers_ExpectedResult() => ExecuteMathExpressionAndAssertResult((left, right) => left / right, 2);

    [Fact]
    public void Equal_DifferentNumbers_ExpectedResult() => ExecuteLogicalExpressionAndAssertResult((left, right) => left == right, false);

    [Fact]
    public void NotEqual_DifferentNumbers_ExpectedResult() => ExecuteLogicalExpressionAndAssertResult((left, right) => left != right, true);

    [Fact]
    public void GreaterThan_Numbers_ExpectedResult() => ExecuteLogicalExpressionAndAssertResult((left, right) => left > right, true);
    
    [Fact]
    public void GreaterThanOrEqual_Numbers_ExpectedResult() => ExecuteLogicalExpressionAndAssertResult((left, right) => left >= right, true);

    [Fact]
    public void LessThan_Numbers_ExpectedResult() => ExecuteLogicalExpressionAndAssertResult((left, right) => left < right, false);

    [Fact]
    public void LessThanOrEqual_Numbers_ExpectedResult() => ExecuteLogicalExpressionAndAssertResult((left, right) => left <= right, false);

    private static void ExecuteMathExpressionAndAssertResult(Func<Number, Number, Number> calcFun, decimal expected)
    {
        string leftName = "left", rightName = "right";

        Number
            left = Number.Of(4, leftName),
            right = Number.Of(2, rightName);

        Number result = calcFun(left, right);

        result.Primitive.Should().Be(expected);
        result.Expression.Arguments.Count.Should().Be(2);
        result.Expression.Arguments.First().Name.Should().Be(leftName);
        result.Expression.Arguments.Last().Name.Should().Be(rightName);
    }

    private static void ExecuteLogicalExpressionAndAssertResult(Func<Number, Number, Condition> calcFun, bool expected)
    {
        string leftName = "left", rightName = "right";

        Number
            left = Number.Of(4, leftName),
            right = Number.Of(2, rightName);

        Condition result = calcFun(left, right);

        result.Primitive.Should().Be(Convert.ToDecimal(expected));
        result.Expression.Arguments.Count.Should().Be(2);
        result.Expression.Arguments.First().Name.Should().Be(leftName);
        result.Expression.Arguments.Last().Name.Should().Be(rightName);
    }
}
