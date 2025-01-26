using System.Collections.Immutable;

namespace CompiledTemplateEngine.Runtime.Interfaces;

public interface ITemplateExecutionHandler {
    Task Execute(
        object requestValue,
        ImmutableDictionary<string, object> variables,
        IServiceProvider serviceProvider,
        ITemplateOutputWriter writer,
        ITemplateExecutionContext? parentContext);
}