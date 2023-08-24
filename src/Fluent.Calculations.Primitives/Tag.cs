namespace Fluent.Calculations.Primitives
{
    public class Tag
    {
        private Tag() { }
        public static Tag Create(string name) => new Tag { Name = name };
        public string Name { get; private set; } = "Undefined";
    }
}