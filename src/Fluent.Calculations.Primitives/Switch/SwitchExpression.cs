namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using Fluent.Calculations.Primitives.Utils;
using System;
using System.Text;

public static class SwitchExpression<T, TReturn>
        where T : struct, Enum
        where TReturn : class, IValueProvider, new()
{
    public static SwichBuilder For(Option<T> checkValue) => new(checkValue, new Dictionary<T, TReturn>());

    public sealed class SwichBuilder
    {
        private readonly Option<T> checkValue;
        private readonly IDictionary<T, TReturn> switchCases;

        private SwichBuilder() {
            checkValue = new Option<T>();
            switchCases = new Dictionary<T, TReturn>();
        }

        internal SwichBuilder(Option<T> checkValue, IDictionary<T, TReturn> switchCases)
        {
            this.checkValue = checkValue;
            this.switchCases = switchCases;
        }

        public CaseBuilder Case(T caseValue, params T[] otherCaseValues) =>
            new(checkValue, switchCases, ArrayHelpers.Concat(caseValue, otherCaseValues));
    }

    public sealed class CaseBuilder
    {
        private readonly Option<T> checkValue;
        private readonly T[] caseValues;
        private readonly IDictionary<T, TReturn> switchCases;

        private CaseBuilder()
        {
            checkValue = new Option<T>();
            caseValues = [];
            switchCases = new Dictionary<T, TReturn>();
        }

        internal CaseBuilder(Option<T> checkValue, IDictionary<T, TReturn> switchCases, T[] caseValues)
        {
            this.checkValue = checkValue;
            this.switchCases = switchCases;
            this.caseValues = caseValues;
        }

        public NextCaseBuilder Return(TReturn returnValue)
        {
            foreach (T caseValue in caseValues)
                switchCases.Add(caseValue, returnValue);

            return new NextCaseBuilder(checkValue, switchCases);
        }
    }

    public sealed class NextCaseBuilder
    {
        private readonly Option<T> checkValue;
        private readonly IDictionary<T, TReturn> switchCases;

        public CaseBuilder Case(T caseValue, params T[] otherCaseValues) =>
            new(checkValue, switchCases, ArrayHelpers.Concat(caseValue, otherCaseValues));

        private NextCaseBuilder() {
            checkValue = new Option<T>();
            switchCases = new Dictionary<T, TReturn>();
        }

        internal NextCaseBuilder(Option<T> checkValue, IDictionary<T, TReturn> switchCases)
        {
            this.checkValue = checkValue;
            this.switchCases = switchCases;
        }

        public SwitchEvaluator Default(TReturn defaultValue) => new(checkValue, switchCases, defaultValue);
    }

    internal static class SwitchExpressionBodyComposer
    {
        public static string Compose(IDictionary<T, TReturn> switchCases, Option<T> checkValue, TReturn defaultValue)
        {
            StringBuilder bodyBuilder = new();

            bodyBuilder.AppendLine($"Switch({checkValue})");

            foreach (KeyValuePair<T, TReturn> item in switchCases)
                bodyBuilder.AppendLine($"{item.Key} => {ComposeReturnBlock(item.Value)}");

            bodyBuilder.AppendLine($"default => {ComposeReturnBlock(defaultValue)}");

            return bodyBuilder.ToString();

            static string ComposeReturnBlock(TReturn value) => value.Origin == ValueOriginType.Constant ?
                value.PrimitiveString : $"{value.PrimitiveString} ({value.Name})" ?? StringConstants.NaN;
        }
    }

    public sealed class SwitchEvaluator
    {
        private readonly IDictionary<T, TReturn> switchCases;
        private readonly Option<T> checkValue;
        private readonly TReturn defaultValue;

        public SwitchEvaluator(Option<T> checkValue, IDictionary<T, TReturn> switchCases, TReturn defaultValue)
        {
            this.checkValue = checkValue;
            this.switchCases = switchCases;
            this.defaultValue = defaultValue;
        }

        private SwitchEvaluator() 
        {
            checkValue = new Option<T>();
            defaultValue = new TReturn();
            switchCases = new Dictionary<T, TReturn>();
        }

        internal TReturn GetResult(string name)
        {
            if (switchCases.TryGetValue(checkValue, out TReturn? foundCase))
                return MakeResult(foundCase);

            return MakeResult(defaultValue);

            TReturn MakeResult(TReturn resultValue) => MakeSwitchResult(resultValue, defaultValue, name);
        }

        private TReturn MakeSwitchResult(TReturn value, TReturn defaultValue, string name)
        {
            List<IValue> expressionArguments = [checkValue];

            TReturn[] nonConstanArguments = switchCases.Values.Where(AsssumeNotInlineConstant).ToArray();

            expressionArguments.AddRange(nonConstanArguments);

            string expressionBody = SwitchExpressionBodyComposer.Compose(switchCases, checkValue, defaultValue);

            ExpressionNode expressionNode = new ExpressionNode(expressionBody, ExpressionNodeType.Switch).WithArguments(expressionArguments);

            return (TReturn)value.MakeOfThisType(MakeValueArgs.Compose(name, expressionNode, value.Primitive, ValueOriginType.Evaluation));

            static bool AsssumeNotInlineConstant(TReturn v) => v.Origin != ValueOriginType.Constant;
        }
    }
}
