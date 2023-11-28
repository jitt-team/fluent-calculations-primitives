using Fluent.Calculations.Primitives.BaseTypes;
using System;
using System.Linq.Expressions;
using static Fluent.Calculations.Primitives.Tests.Evaluate<TValue>;

namespace Fluent.Calculations.Primitives.Tests
{
    public class Experiments
    {

        [Fact]
        public void Test()
        {
            Number Value = 10;

            Number result = Result.Of(Evaluate<Number, Number>
                .Switch(Value)
                    .Case(Number.Of(10)).Return(Number.Of(15))
                    .Case(Number.Of(20)).Return(Number.Of(30))
                    .Default(Number.Of(20)));
        }
    }
    public static class Evaluate<TCase, TReturn>
    {
        public static SwichExpressionBuilder<TCase, TReturn> Switch(TCase value)
        {
            return new SwichExpressionBuilder<TCase, TReturn>();
        }

        public sealed class ThenBuilder<TCase, TReturn>
        {
            public NextWhenBuilder<TCase, TReturn> Return(TReturn number)
            {
                return new NextWhenBuilder<TCase, TReturn>();
            }
        }

        public sealed class NextWhenBuilder<TCase, TReturn>
        {
            internal Expression<Func<TCase>> Default(TReturn number)
            {
                Expression<Func<TCase>> resultExpression = null;

                return resultExpression;
            }

            internal ThenBuilder<TCase, TReturn> Case(TCase number)
            {
                return new ThenBuilder<TCase, TReturn>();
            }
        }

        public sealed class SwichExpressionBuilder<TCase, TReturn>
        {
            private readonly Dictionary<TCase, TReturn> switchCases = new();

            public ThenBuilder<TCase, TReturn> Case(TCase caseValue)
            {
                return new ThenBuilder<TCase, TReturn>(switchCases, );
            }
        }
    }


}
