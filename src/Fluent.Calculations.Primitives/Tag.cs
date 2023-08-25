namespace Fluent.Calculations.Primitives
{
    public class Tag
    {
        public required string Name { get; init; }

        private Tag() { }

        public static Tag Create(string name) => new() { Name = name };
    }
}