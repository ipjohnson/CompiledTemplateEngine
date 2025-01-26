using CompiledTemplateEngine.Runtime.Interfaces;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace CompiledTemplateEngine.Runtime.Helpers;

public class DynamicTemplateRenderHelper : ITemplateHelper {
    private readonly ConcurrentDictionary<string, TemplateExecutionFunction?> _executionFunctions = new();

    public async ValueTask<object?> Execute(ITemplateExecutionContext handlerDataContext, params object[] arguments) {
        if (arguments.Length == 2) {
            TemplateExecutionFunction? function;

            if (arguments[1] is TemplateExecutionFunction) {
                function = (TemplateExecutionFunction)arguments[1];
            }
            else {
                function = _executionFunctions.GetOrAdd(arguments[0].ToString()!,
                    templateName => handlerDataContext.ExecutionService.FindTemplateExecutionFunction(templateName));
            }

            if (function != null) {
                await function(
                    arguments[0],
                    ImmutableDictionary<string, object>.Empty, 
                    handlerDataContext.RequestServiceProvider,
                    handlerDataContext.Writer,
                    handlerDataContext);
            }
        }

        return string.Empty;
    }
}