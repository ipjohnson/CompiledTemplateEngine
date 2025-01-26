using System.Text.Json;
using CompiledTemplateEngine.Runtime.Interfaces;

namespace CompiledTemplateEngine.Runtime.Helpers.String;

public class JsonHelper : ITemplateHelper {
    public ValueTask<object?> Execute(ITemplateExecutionContext handlerDataContext, params object[] arguments) {
        if (arguments.Length == 0) {
            return new ValueTask<object?>("");
        }

        if (arguments.Length == 1) {
            return new ValueTask<object?>(
                new SafeString(
                    JsonSerializer.Serialize(arguments[0])
                ));
        }

        return new ValueTask<object?>(
            new SafeString(
                JsonSerializer.Serialize(arguments))
        );
    }
}