namespace Fluent.Calculations.Primitives;
using Fluent.Calculations.Primitives.BaseTypes;
using Fluent.Calculations.Primitives.Expressions;
using System.Runtime.CompilerServices;

public static class Switch<TValue> where TValue : class, IValueProvider, new()
{
    public static SwichExpressionBuilder For(IValue checkValue) => new SwichExpressionBuilder(checkValue, new Dictionary<IValue, TValue>());

    public sealed class CaseBuilder
    {
        private readonly IValue checkValue;
        private readonly IValue caseValue;
        private readonly IDictionary<IValue, TValue> switchCases;

        public CaseBuilder(IValue checkValue, IDictionary<IValue, TValue> switchCases, IValue caseValue)
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

        public NextCaseBuilder(IValue checkValue, IDictionary<IValue, TValue> switchCases)
        {
            this.checkValue = checkValue;
            this.switchCases = switchCases;
        }

        internal TValue Default(TValue defaultValue, [CallerMemberName] string name = StringConstants.NaN)
        {
            TValue? result;

            if (!switchCases.TryGetValue(checkValue, out result))
                result = defaultValue;

            // TODO : compose switch pseudo-code body
            string expressionBody = "Switch body TODO";
            List<IValue> expressionArguments = new() { checkValue };
            // TODO : enlist parameter and expression arguments

            ExpressionNode expressionNode = new ExpressionNode(expressionBody, ExpressionNodeType.Switch).WithArguments(expressionArguments);

            return (TValue)result.MakeOfThisType(MakeValueArgs.Compose(name, expressionNode, result.Primitive, ValueOriginType.Evaluation));
        }

        internal CaseBuilder Case(IValue caseValue) => new CaseBuilder(checkValue, switchCases, caseValue);
    }

    public sealed class SwichExpressionBuilder
    {
        private IValue checkValue;
        private readonly IDictionary<IValue, TValue> switchCases;

        public SwichExpressionBuilder(IValue checkValue, IDictionary<IValue, TValue> switchCases)
        {
            this.checkValue = checkValue;
            this.switchCases = switchCases;
        }

        public CaseBuilder Case(IValue caseValue) => new CaseBuilder(checkValue, switchCases, caseValue);
    }
}
