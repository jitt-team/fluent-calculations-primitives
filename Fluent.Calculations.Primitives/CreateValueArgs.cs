namespace Fluent.Calculations.Primitives
{
    public class CreateValueArgs
    {
        public string Name { get; private set; } = "NotProvided";

        public ExpressionNode Expresion { get; private set; } = ExpressionNode.Default;

        public decimal PrimitiveValue { get; private set; }

        public bool IsConstant { get; private set; }

        public ArgumentsList Arguments { get; private set; } = ArgumentsList.Empty;

        public TagsList Tags { get; private set; } = TagsList.Empty;

        internal static CreateValueArgs Compose(string name, ExpressionNode expression, decimal value) =>
            new CreateValueArgs
            {
                Name = name,
                Expresion = expression,
                PrimitiveValue = value,
                IsConstant = false
            };

        public CreateValueArgs WithArguments(ArgumentsList argumentsList)
        {
            Arguments = argumentsList;
            return this;
        }
        public CreateValueArgs WithArguments(IValue[] arguments) => WithArguments(ArgumentsList.CreateFrom(arguments));

        public CreateValueArgs WithArguments(IValue a, params IValue[] b) => WithArguments(new[] { a }.Concat(b).ToArray());

        public CreateValueArgs WithTags(params Tag[] tags)
        {
            Tags = new TagsList(tags);
            return this;
        }
    }
}
