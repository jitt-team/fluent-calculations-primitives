namespace Fluent.Calculations.Primitives.Expressions.Capture;
using System.Linq.Expressions;
using System.Reflection;

internal class MemberAccessExtractor : IMemberAccessExtractor
{
    public List<object> CaptureParameterMember(MemberExpression expression)
    {
        // TODO : Any way to make this once per member and not one each usage? Maybe invoke much later?
        if (IsInputParameter(expression.Member))
            return ToListResult(new CapturedInputMember(ValueExpressionInvoker.DynamicInvoke(expression), expression.Member.Name));

        // Don't invoke to conserve performance, perhaps could be a DebugMode to map out full tree
        else if (IsEvaluation(expression.Member))
            return ToListResult(new CapturedEvaulationMember(((PropertyInfo)expression.Member).Name));

        return new List<object>(); // TODO : Throw exception early, explain why it shouldn't happen
    }

    private bool IsInputParameter(MemberInfo member) => member.MemberType == MemberTypes.Field
        || member.MemberType == MemberTypes.Property && ((PropertyInfo)member).CanWrite;

    private bool IsEvaluation(MemberInfo member) => member.MemberType == MemberTypes.Property && !((PropertyInfo)member).CanWrite;

    private static List<object> ToListResult(object obj) => new List<object> { obj };
}
