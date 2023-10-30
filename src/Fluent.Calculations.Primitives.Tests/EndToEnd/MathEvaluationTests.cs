using Fluent.Calculations.DotNetGraph;
using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;

namespace Fluent.Calculations.Primitives.Tests.Integration
{

    public class MathEvaluationTests
    {
        [Fact]
        public void MathEvaluation_IsExpectedResult_AndExpectedArgumentTree()
        {
            MathEvaluations calculation = new()
            {
                ConstantOne = Number.Of(2),
                ConstantTwo = Number.Of(3),
                ConstantThree = Number.Of(4)
            };

            Number result = calculation.ToResult();

            result.Primitive.Should().Be(8);
            IValue mathArgument = result.Expression.Arguments.First(a => a.Name.Equals(nameof(calculation.MathMin)));
            mathArgument.Should().NotBeNull();
            mathArgument.Expression.Arguments.Should().HaveCount(2);
            mathArgument.Expression.Arguments.First().Name.Should().Be(nameof(calculation.ConstantOne));
            mathArgument.Expression.Arguments.Last().Name.Should().Be(nameof(calculation.ConstantTwo));
        }
    }

    internal class MathEvaluations : EvaluationContext<Number>
    {
        public MathEvaluations() : base(new EvaluationOptions { AlwaysReadNamesFromExpressions = true }) { }

        public Number
            ConstantOne = Number.Zero,
            ConstantTwo = Number.Zero,
            ConstantThree = Number.Zero;

        public Number MathMin => Evaluate(() => ValueMath.Min(ConstantOne, ConstantTwo));

        Number EvaluationTwo => Evaluate(() => MathMin * ConstantThree);

        public override Number Return() => EvaluationTwo;
    }
}
