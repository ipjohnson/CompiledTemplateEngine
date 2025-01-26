using System.Collections.Immutable;

namespace CompiledTemplateEngine.Runtime.Interfaces;

public delegate Task TemplateExecutionFunction(
    object templateData,
    ImmutableDictionary<string,object> customData,
    IServiceProvider serviceProvider,
    ITemplateOutputWriter writer,
    ITemplateExecutionContext? parentContext);

public interface ITemplateExecutionService {

    Task<string> Execute(
        string templateName,
        object? templateData,
        ImmutableDictionary<string, object>? customData = null,
        IServiceProvider? serviceProvider = null,
        ITemplateExecutionContext? parentContext = null);
    
    Task Execute(
        string templateName,
        object? templateData,
        ITemplateOutputWriter writer,
        ImmutableDictionary<string, object>? customData = null,
        IServiceProvider? serviceProvider = null,
        ITemplateExecutionContext? parentContext = null);

    TemplateExecutionFunction? FindTemplateExecutionFunction(string templateName);
}
