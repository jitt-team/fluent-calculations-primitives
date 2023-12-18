namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

public static class SwitchExpression<T, TReturn>
        where T : struct, Enum
        where TReturn : class, IValueProvider, new()
{
    public static SwitchBuilder For(Option<T> checkValue) => new(checkValue, new Dictionary<T, ReturnValue>());

    public sealed class SwitchBuilder
    {
        private readonly Option<T> checkValue;
        private readonly IDictionary<T, ReturnValue> switchCases;

        private SwitchBuilder()
        {
            checkValue = new Option<T>();
            switchCases = new Dictionary<T, ReturnValue>();
        }

        internal SwitchBuilder(Option<T> checkValue, IDictionary<T, ReturnValue> switchCases)
        {
            this.checkValue = checkValue;
            this.switchCases = switchCases;
        }

        public ReturnBuilder Case(T caseValue, params T[] otherCaseValues) =>
            new(checkValue, switchCases, ArrayHelpers.Concat(caseValue, otherCaseValues));
    }

    public sealed class ReturnBuilder
    {
        private readonly Option<T> checkValue;
        private readonly T[] caseValues;
        private readonly IDictionary<T, ReturnValue> switchCases;

        private ReturnBuilder()
        {
            checkValue = new Option<T>();
            caseValues = [];
            switchCases = new Dictionary<T, ReturnValue>();
        }

        internal ReturnBuilder(Option<T> checkValue, IDictionary<T, ReturnValue> switchCases, T[] caseValues)
        {
            this.checkValue = checkValue;
            this.switchCases = switchCases;
            this.caseValues = caseValues;
        }

        public CaseBuilder Return(decimal primitiveValue, [CallerArgumentExpression(nameof(primitiveValue))] string valueBody = StringConstants.NaN) =>
            Return(new ReturnValue(primitiveValue, valueBody));

        public CaseBuilder Return(Func<TReturn> returnValueFunc, [CallerArgumentExpression(nameof(returnValueFunc))] string funcBody = StringConstants.NaN) =>
            Return(new ReturnValue(returnValueFunc, funcBody));

        private CaseBuilder Return(ReturnValue returnValue)
        {
            foreach (T caseValue in caseValues)
                switchCases.Add(caseValue, returnValue);

            return new CaseBuilder(checkValue, switchCases);
        }
    }

    public sealed class CaseBuilder
    {
        private readonly Option<T> checkValue;
        private readonly IDictionary<T, ReturnValue> switchCases;

        public ReturnBuilder Case(T caseValue, params T[] otherCaseValues) =>
            new(checkValue, switchCases, ArrayHelpers.Concat(caseValue, otherCaseValues));

        private CaseBuilder()
        {
            checkValue = new Option<T>();
            switchCases = new Dictionary<T, ReturnValue>();
        }

        internal CaseBuilder(Option<T> checkValue, IDictionary<T, ReturnValue> switchCases)
        {
            this.checkValue = checkValue;
            this.switchCases = switchCases;
        }
        public ResultEvaluator Default(decimal primitiveValue, [CallerArgumentExpression(nameof(primitiveValue))] string valueBody = StringConstants.NaN) =>
            new(checkValue, switchCases, new ReturnValue(primitiveValue, valueBody));

        public ResultEvaluator Default(Func<TReturn> defaultValue, [CallerArgumentExpression(nameof(defaultValue))] string funcBody = StringConstants.NaN) =>
            new(checkValue, switchCases, new ReturnValue(defaultValue, funcBody));
    }

    public sealed class ResultEvaluator
    {
        private readonly IDictionary<T, ReturnValue> switchCases;
        private readonly Option<T> checkValue;
        private readonly ReturnValue defaultValue;
        private static readonly TReturn DefaultProvider = new();

        internal ResultEvaluator(Option<T> checkValue, IDictionary<T, ReturnValue> switchCases, ReturnValue defaultValue)
        {
            this.checkValue = checkValue;
            this.switchCases = switchCases;
            this.defaultValue = defaultValue;
        }

        private ResultEvaluator()
        {
            checkValue = new Option<T>();
            defaultValue = new ReturnValue(() => new TReturn(), StringConstants.NaN);
            switchCases = new Dictionary<T, ReturnValue>();
        }

        internal TReturn GetResult(string name)
        {
            if (switchCases.TryGetValue(checkValue, out ReturnValue? foundCase))
                return MakeResult(foundCase);

            return MakeResult(defaultValue);

            TReturn MakeResult(ReturnValue resultValue) => MakeSwitchResult(resultValue, defaultValue, name);
        }

        private TReturn MakeSwitchResult(ReturnValue switchResult, ReturnValue defaultValue, string name)
        {
            List<IValue> expressionArguments = [checkValue];
            decimal resultPrimitive;

            if (switchResult.IsPrimitive)
                resultPrimitive = switchResult.PrimitiveValue;
            else
            {
                TReturn resultValue = switchResult.Get();
                resultPrimitive = resultValue.Primitive;
                expressionArguments.Add(resultValue);
            }

            string expressionBody = SwitchExpressionBodyComposer.Compose(switchCases, checkValue, defaultValue);
            ExpressionNode expressionNode = new ExpressionNode(expressionBody, ExpressionNodeType.Switch).WithArguments(expressionArguments);
            return (TReturn)DefaultProvider.MakeOfThisType(MakeValueArgs.Compose(name, expressionNode, resultPrimitive, ValueOriginType.Operation));
        }
    }

    internal static class SwitchExpressionBodyComposer
    {
        public static string Compose(IDictionary<T, ReturnValue> switchCases, Option<T> checkValue, ReturnValue defaultValue)
        {
            StringBuilder bodyBuilder = new();

            bodyBuilder.AppendLine($"Switch({checkValue} – {checkValue.PrimitiveString})");

            foreach (KeyValuePair<T, ReturnValue> item in switchCases)
                bodyBuilder.AppendLine($"   {item.Key} => {ComposeReturnBlock(item.Value)}");

            bodyBuilder.AppendLine($"   default => {ComposeReturnBlock(defaultValue)}");

            return bodyBuilder.ToString();

            static string ComposeReturnBlock(ReturnValue value) => value.IsPrimitive ?
                value.PrimitiveValue.ToString() : $"({value.Name})" ?? StringConstants.NaN;
        }
    }

    internal class ReturnValue
    {
        private ReturnValue() { }

        private ReturnValue(string name) => Name = name;

        public ReturnValue(Func<TReturn> getter, string name) : this(name) => Get = getter;

        public ReturnValue(decimal primitive, string name) : this(name)
        {
            IsPrimitive = true;
            PrimitiveValue = primitive;
        }

        public Func<TReturn> Get { get; private set; } = () => new TReturn();

        public decimal PrimitiveValue { get; private set; }

        public string Name { get; private set; } = StringConstants.NaN;

        public bool IsPrimitive { get; private set; }
    }
}
