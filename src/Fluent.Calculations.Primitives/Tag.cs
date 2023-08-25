namespace Fluent.Calculations.Primitives
{
    public class Tag
    {
        private Tag() { }

        public static Tag Create(string name) => new() { Name = name };

        public required string Name { get; init; }
    }
}