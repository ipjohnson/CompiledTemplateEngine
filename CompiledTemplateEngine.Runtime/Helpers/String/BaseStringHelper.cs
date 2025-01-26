using CompiledTemplateEngine.Runtime.Interfaces;

namespace CompiledTemplateEngine.Runtime.Helpers.String;

public abstract class BaseStringHelper : ITemplateHelper {
    public ValueTask<object?> Execute(ITemplateExecutionContext handlerDataContext, params object[] arguments) {
        if (arguments is { Length: > 0 } && arguments[0] != null) {
            var returnValue = 
                AugmentString(arguments[0].ToString() ?? string.Empty);

            return new ValueTask<object?>(returnValue);
        }

        return new ValueTask<object?>(false);
    }

    protected abstract object AugmentString(string value);
}