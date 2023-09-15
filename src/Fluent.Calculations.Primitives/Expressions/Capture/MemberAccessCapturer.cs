namespace Fluent.Calculations.Primitives.Expressions.Capture;
using System.Linq.Expressions;

internal class MemberAccessCapturer : IMemberAccessCapturer
{
    private readonly IMemberAccessReflectionProvider reflectionProvider;
    
    public MemberAccessCapturer() : this(new MemberAccessReflectionProvider()) { }

    public MemberAccessCapturer(IMemberAccessReflectionProvider reflectionProvider) => this.reflectionProvider = reflectionProvider;

    public List<object> Capture(MemberExpression expression)
    {
        // TODO: Any way to make this once per member and not one each usage? Maybe invoke much later?
        // TODO: Maybe we can capture just expressions to members and then invoke the at the end just once?
        if (reflectionProvider.IsInputMember(expression.Member))
            return ToList(new CapturedInputMember(reflectionProvider.GetValue(expression), reflectionProvider.GetPropertyName(expression)));

        // TODO: Don't invoke to conserve performance, perhaps could be a DebugMode to map out full tree
        else if (reflectionProvider.IsEvaluationMember(expression.Member))
            return ToList(new CapturedEvaulationMember(reflectionProvider.GetPropertyName(expression)));

        // TODO: Throw exception early, explain why it shouldn't happen
        return new List<object>();
    }

    private static List<object> ToList(object obj) => new List<object> { obj };
}