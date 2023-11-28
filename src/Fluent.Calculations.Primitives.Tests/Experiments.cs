using Fluent.Calculations.Primitives.BaseTypes;
using FluentAssertions;
using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Fluent.Calculations.Primitives.Tests
{
    public class Experiments
    {

        [Fact]
        public void Test()
        {
            Number Value = 20;

            // Number result = Result.Of();
            Number result = SwitchExpression<Number, Number>
                .Switch(Value)
                    .Case(Number.Of(10)).Return(Number.Of(15))
                    .Case(Number.Of(20)).Return(Number.Of(30))
                    .Default(Number.Of(20));

            result.Primitive.Should().Be(30);
        }
    }
    public static class SwitchExpression<TCase, TReturn>
    {
        public static SwichExpressionBuilder<TCase, TReturn> Switch(TCase checkValue)
        {
            return new SwichExpressionBuilder<TCase, TReturn>(checkValue, new Dictionary<TCase, TReturn>());
        }

        public sealed class CaseBuilder<TCase, TReturn>
        {
            private readonly TCase checkValue;
            private readonly TCase caseValue;
            private readonly IDictionary<TCase, TReturn> switchCases;

            public CaseBuilder(TCase checkValue, IDictionary<TCase, TReturn> switchCases, TCase caseValue)
            {
                this.checkValue = checkValue;
                this.switchCases = switchCases;
                this.caseValue = caseValue;
            }

            public NextCaseBuilder<TCase, TReturn> Return(TReturn returnValue)
            {
                switchCases.Add(caseValue,returnValue);
                return new NextCaseBuilder<TCase, TReturn>(checkValue, switchCases, caseValue);
            }
        }

        public sealed class NextCaseBuilder<TCase, TReturn>
        {
            private readonly TCase checkValue;
            private readonly IDictionary<TCase, TReturn> switchCases;

            public NextCaseBuilder(TCase? checkValue, IDictionary<TCase, TReturn> switchCases, TCase? caseValue)
            {
                this.checkValue = checkValue;
                this.switchCases = switchCases;
            }

            // internal Expression<Func<TCase>> Default(TReturn defaultValue)
            internal TReturn Default(TReturn defaultValue)
            {
                // Expression<Func<TCase>> resultExpression = null;

                if (switchCases.TryGetValue(checkValue, out TReturn? result))
                    return result;

                return defaultValue;

                // return resultExpression;
            }

            internal CaseBuilder<TCase, TReturn> Case(TCase caseValue)
            {
                return new CaseBuilder<TCase, TReturn>(checkValue, switchCases, caseValue);
            }
        }

        public sealed class SwichExpressionBuilder<TCase, TReturn>
        {
            private TCase checkValue;
            private readonly IDictionary<TCase, TReturn> switchCases;

            public SwichExpressionBuilder(TCase checkValue, IDictionary<TCase, TReturn> switchCases)
            {
                this.checkValue = checkValue;
                this.switchCases = switchCases;
            }

            public CaseBuilder<TCase, TReturn> Case(TCase caseValue)
            {
                return new CaseBuilder<TCase, TReturn>(checkValue, switchCases, caseValue);
            }
        }
    }


}
