namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Text;

public static class SwitchExpression<TValue> where TValue : class, IValueProvider, new()
{
    public static SwichBuilder For(IValue checkValue) => new SwichBuilder(checkValue, new Dictionary<IValue, TValue>());

    public sealed class SwichBuilder
    {
        private IValue checkValue;
        private readonly IDictionary<IValue, TValue> switchCases;

        private SwichBuilder() { }

        internal SwichBuilder(IValue checkValue, IDictionary<IValue, TValue> switchCases)
        {
            this.checkValue = checkValue;
            this.switchCases = switchCases;
        }

        public CaseBuilder Case(IValue caseValue) => new CaseBuilder(checkValue, switchCases, caseValue);
    }

    public sealed class CaseBuilder
    {
        private readonly IValue checkValue;
        private readonly IValue caseValue;
        private readonly IDictionary<IValue, TValue> switchCases;

        private CaseBuilder() { }

        internal CaseBuilder(IValue checkValue, IDictionary<IValue, TValue> switchCases, IValue caseValue)
        {
            this.checkValue = checkValue;
            this.switchCases = switchCases;
            this.caseValue = caseValue;
        }

        public NextCaseBuilder Return(TValue returnValue)
        {
            switchCases.Add(caseValue, returnValue);
            return new NextCaseBuilder(checkValue, switchCases);
        }
    }

    public sealed class NextCaseBuilder
    {
        private readonly IValue checkValue;
        private readonly IDictionary<IValue, TValue> switchCases;

        public CaseBuilder Case(IValue caseValue) => new CaseBuilder(checkValue, switchCases, caseValue);

        private NextCaseBuilder() { }

        internal NextCaseBuilder(IValue checkValue, IDictionary<IValue, TValue> switchCases)
        {
            this.checkValue = checkValue;
            this.switchCases = switchCases;
        }

        public SwitchEvaluator Default(TValue defaultValue) => new SwitchEvaluator(checkValue, switchCases, defaultValue);

    }

    internal static class SwitchExpressionBodyComposer
    {
        public static string Compose(IDictionary<IValue, TValue> switchCases, IValue checkValue, TValue defaultValue)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Switch({checkValue})");

            foreach (KeyValuePair<IValue, TValue> item in switchCases)
            {
                sb.AppendLine($"{item.Key.Primitive} => {ComposeReturnBlock(item.Value)}");
            }

            sb.AppendLine($"default => {ComposeReturnBlock(defaultValue)}");

            return sb.ToString();

            static string ComposeReturnBlock(TValue value) => value.Origin == ValueOriginType.Constant ?
                value.Primitive.ToString() : value.ToString() ?? StringConstants.NaN;
        }
    }

    public sealed class SwitchEvaluator
    {
        private IDictionary<IValue, TValue> switchCases;
        private IValue checkValue;
        private TValue defaultValue;

        public SwitchEvaluator(IValue checkValue, IDictionary<IValue, TValue> switchCases, TValue defaultValue)
        {
            this.checkValue = checkValue;
            this.switchCases = switchCases;
            this.defaultValue = defaultValue;
        }

        private SwitchEvaluator() { }

        internal TValue GetResult(string name)
        {
            if (switchCases.TryGetValue(checkValue, out TValue? foundCase))
                return MakeResult(foundCase);

            return MakeResult(defaultValue);

            TValue MakeResult(TValue resultValue) => MakeSwitchResult(resultValue, defaultValue, name);
        }

        private TValue MakeSwitchResult(TValue value, TValue defaultValue, string name)
        {
            List<IValue> expressionArguments = new() { checkValue };
            var nonConstanArguments = switchCases.Values.Where(v => v.Origin != ValueOriginType.Constant);
            // expressionArguments.AddRange(nonConstanArguments);

            string expressionBody = SwitchExpressionBodyComposer.Compose(switchCases, checkValue, defaultValue);

            ExpressionNode expressionNode = new ExpressionNode(expressionBody, ExpressionNodeType.Switch).WithArguments(expressionArguments);

            return (TValue)value.MakeOfThisType(MakeValueArgs.Compose(name, expressionNode, value.Primitive, ValueOriginType.Evaluation));
        }
    }
}
