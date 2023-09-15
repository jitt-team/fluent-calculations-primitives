using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions.Capture;
using FluentAssertions;
using System.Linq.Expressions;

namespace Fluent.Calculations.Primitives.Tests.Expressions;

public class MultiPartExpressionMemberExtractorTests
{
    private readonly MultiPartExpressionMemberExtractor expressionMemberExtractor = new MultiPartExpressionMemberExtractor();
    private readonly MemberAccessReflectionProvider memberAccessReflectionProvider = new MemberAccessReflectionProvider();

    [Fact]
    public void ExtractConditionalExpressionMembers_ReturnsThreeMember()
    {
        Number
            testValue1 = Number.Of(1, "TEST-VALUE-1"),
            testValue2 = Number.Of(2, "TEST-VALUE-2");

        Condition 
            testCondition = Condition.True("TEST-CONDITION-1");

        Expression<Func<Number>> testExpression = () => testCondition ? testValue1 : testValue2;
        ConditionalExpression testConditionalExpression = (ConditionalExpression)testExpression.Body;
        Expression[] memberResults = expressionMemberExtractor.ExtractConditionalExpressionMembers(testConditionalExpression);
        UnaryExpression testMemberConversion = (UnaryExpression)memberResults[0];

        IValue
            resultCondition = memberAccessReflectionProvider.GetValue(testMemberConversion.Operand),
            resultValue1 = memberAccessReflectionProvider.GetValue(memberResults[1]),
            resultValue2 = memberAccessReflectionProvider.GetValue(memberResults[2]);

        resultCondition.Name.Should().Be(testCondition.Name);
        resultValue1.Name.Should().Be(testValue1.Name);
        resultValue2.Name.Should().Be(testValue2.Name);
    }

    [Fact]
    public void ExtractBinaryExpression_Returns_TwoMembers()
    {
        Number
            testValue1 = Number.Of(1, "TEST-VALUE-1"),
            testValue2 = Number.Of(2, "TEST-VALUE-2");

        Expression<Func<Number>> testExpression = () => testValue1 + testValue2;
        BinaryExpression testBinaryExpression = (BinaryExpression)testExpression.Body;

        Expression[] memberResults = expressionMemberExtractor.ExtractBinaryExpressionMembers(testBinaryExpression);

        IValue
            resultValue1 = memberAccessReflectionProvider.GetValue(memberResults[0]),
            resultValue2 = memberAccessReflectionProvider.GetValue(memberResults[1]);

        resultValue1.Name.Should().Be(testValue1.Name);
        resultValue2.Name.Should().Be(testValue2.Name);
    }

    [Fact]
    public void IsBinaryExpression_IncorrectType_ReturnFalse() =>
        expressionMemberExtractor.IsBinaryExpression(ExpressionType.Convert).Should().BeFalse();

    [Theory]
    [MemberData(nameof(BinaryExpressionOperators))]
    public void IsBinaryExpression_CorrectType_ReturnTrue(ExpressionType testType) =>
        expressionMemberExtractor.IsBinaryExpression(testType).Should().BeTrue();

    public static IEnumerable<object[]> BinaryExpressionOperators => new[]
    {
        ExpressionType.Add,
        ExpressionType.Subtract,
        ExpressionType.Multiply,
        ExpressionType.Divide,
        ExpressionType.GreaterThan,
        ExpressionType.LessThan,
        ExpressionType.GreaterThanOrEqual,
        ExpressionType.LessThanOrEqual,
        ExpressionType.Modulo,
        ExpressionType.Power,
        ExpressionType.Equal,
        ExpressionType.NotEqual,
        ExpressionType.Add,
        ExpressionType.AndAlso,
        ExpressionType.OrElse,
        ExpressionType.Or,
    }.Select(testType => new object[] { testType });
}
