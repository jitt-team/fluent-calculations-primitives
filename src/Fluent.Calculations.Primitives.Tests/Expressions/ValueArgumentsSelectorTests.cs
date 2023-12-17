namespace Fluent.Calculations.Primitives.Tests.Expressions;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using FluentAssertions;

public class ValueArgumentsSelectorTests
{
    [Fact]
    public void MultipleOperators_ReturnsOnlyArguments()
    {
        ValueArgumentsSelector selector = new();
        Number testResult = GetResult();
        IValue[] arguments = selector.Select(testResult);

        arguments.Length.Should().Be(4);
        arguments[0].Name.Should().Be("one");
        arguments[1].Name.Should().Be("two");
        arguments[2].Name.Should().Be("three");
        arguments[3].Name.Should().Be("SomeEvaluaton");
    }

    private Number GetResult()
    {
        Number
            one = Number.Of(1, nameof(one)),
            two = Number.Of(2, nameof(two)),
            three = Number.Of(3, nameof(three));

        return one + two * three - three / MockEvaluationResult(two);
    }

    private Number MockEvaluationResult(Number two) => new(MakeValueArgs.Compose("SomeEvaluaton",
            new ExpressionNode("10", ExpressionNodeType.Lambda), 10, ValueOriginType.Evaluation));
}
