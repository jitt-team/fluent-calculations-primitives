namespace Fluent.Calculations.Primitives
{
    public class CalculationGraph
    {
        public string Identifier { get; private set; }

        public static CalculationGraph BuildFrom(IValue value)
        {

            return new CalculationGraph
            {
                Identifier = value.Name
            };
        }
    }
}