using System.Collections.Concurrent;
using System.Collections.Immutable;
using CompiledTemplateEngine.Runtime.Interfaces;
using CompiledTemplateEngine.Runtime.Utilities;
using DependencyModules.Runtime.Attributes;

namespace CompiledTemplateEngine.Runtime.Engine;

[SingletonService]
public class TemplateExecutionService : ITemplateExecutionService {
    private static readonly object _emptyObject = new object();
    private static readonly ImmutableDictionary<string, object> _emptyData = 
        ImmutableDictionary<string, object>.Empty;
    private readonly ConcurrentDictionary<string, TemplateExecutionFunction> _functionCache = new();
    private readonly IStringBuilderPool _stringBuilderPool;
    private readonly IReadOnlyList<ITemplateExecutionHandlerProvider> _handlerProviders;
    private readonly IServiceProvider _serviceProvider;

    public TemplateExecutionService(IEnumerable<ITemplateExecutionHandlerProvider> handlerProviders,
        IStringBuilderPool stringBuilderPool,
        IServiceProvider serviceProvider) {
        _stringBuilderPool = stringBuilderPool;
        _serviceProvider = serviceProvider;
        _handlerProviders = new List<ITemplateExecutionHandlerProvider>(handlerProviders.Reverse());

        foreach (var templateExecutionHandlerProvider in _handlerProviders) {
            templateExecutionHandlerProvider.TemplateExecutionService = this;
        }
    }


    public async Task<string> Execute(string templateName, object? templateData, ImmutableDictionary<string, object>? customData = null, IServiceProvider? serviceProvider = null, ITemplateExecutionContext? parentContext = null) {
        using var stringBuilder = _stringBuilderPool.Get();
        var output = new StringBuilderTemplateOutputWriter(stringBuilder.Item);
        
        await Execute(templateName, templateData, output, customData, serviceProvider, parentContext);
        
        return output.ToString();
    }

    public async Task Execute(
        string templateName,
        object? templateData,
        ITemplateOutputWriter writer,
        ImmutableDictionary<string, object>? customData = null,
        IServiceProvider? serviceProvider = null,
        ITemplateExecutionContext? parentContext = null) {

        var templateExecutionFunction = FindTemplateExecutionFunction(templateName);

        if (templateExecutionFunction == null) {
            throw new MissingTemplateException(templateName);
        }
        
        await templateExecutionFunction(
            templateData ?? _emptyObject,
            customData ?? _emptyData, 
            serviceProvider ?? _serviceProvider, 
            writer, 
            parentContext);
    }

    public TemplateExecutionFunction? FindTemplateExecutionFunction(string templateName) {
        if (_functionCache.TryGetValue(templateName, out var value)) {
            return value;
        }
        
        foreach (var provider in _handlerProviders) {
            var handler = provider.GetTemplateExecutionHandler(templateName);

            if (handler != null) {
                TemplateExecutionFunction func = handler.Execute;
                
                _functionCache.TryAdd(templateName, func);
                
                return func;
            }
        }

        return null;
    }
}