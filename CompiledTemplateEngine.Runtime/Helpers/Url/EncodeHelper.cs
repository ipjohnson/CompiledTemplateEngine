using System.Web;
using CompiledTemplateEngine.Runtime.Interfaces;

namespace CompiledTemplateEngine.Runtime.Helpers.Url;

public class EncodeHelper : ITemplateHelper {
    public ValueTask<object?> Execute(ITemplateExecutionContext handlerDataContext, params object[] arguments) {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (arguments == null || arguments.Length == 0 || arguments[0] == null) {
            return new ValueTask<object?>("");
        }

        var decodedString = HttpUtility.UrlEncode(arguments[0].ToString());

        return new ValueTask<object?>(decodedString ?? string.Empty);
    }
}