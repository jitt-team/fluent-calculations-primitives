namespace Fluent.Calculations.Primitives
{
    public class TagsList : List<Tag>
    {
        internal TagsList()
        {

        }
        internal TagsList(IEnumerable<Tag> tags) : base(tags)
        {
        }

        internal static TagsList Empty => new TagsList();
    }
}
