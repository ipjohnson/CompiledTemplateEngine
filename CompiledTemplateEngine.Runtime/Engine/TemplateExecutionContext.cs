using System.Collections.Immutable;
using CompiledTemplateEngine.Runtime.Interfaces;

namespace CompiledTemplateEngine.Runtime.Engine;

public class TemplateExecutionContext : ITemplateExecutionContext {
    private ImmutableDictionary<string, object> _variables;

    public TemplateExecutionContext(
        string templateExtension,
        IServiceProvider requestServiceProvider,
        object objectValue,
        IInternalTemplateServices templateServices,
        ITemplateExecutionService executionService,
        IStringEscapeService stringEscapeService,
        ITemplateOutputWriter writer,
        ImmutableDictionary<string, object> variables,
        ITemplateExecutionContext? parentContext) {
        RequestServiceProvider = requestServiceProvider;
        ObjectValue = objectValue;
        TemplateServices = templateServices;
        Writer = writer;
        ParentContext = parentContext;
        StringEscapeService = stringEscapeService;
        ExecutionService = executionService;
        TemplateExtension = templateExtension;
        _variables = variables;
    }

    public ITemplateOutputWriter Writer { get; }

    public ITemplateExecutionService ExecutionService { get; }

    public ImmutableDictionary<string, object> Variables => _variables;

    public void SetVariable(string name, object value) {
        _variables = _variables.Add(name, value);
    }

    public IInternalTemplateServices TemplateServices { get; }

    public IStringEscapeService StringEscapeService { get; }

    public string TemplateExtension { get; }

    public IServiceProvider RequestServiceProvider { get; }

    public ITemplateExecutionContext? ParentContext { get; }

    public object ObjectValue { get; }

    public SafeString GetEscapedString(object value, string propertyName = "", string formattingString = "") {
        return new SafeString(
            StringEscapeService.EscapeString(
                TemplateServices.DataFormattingService.FormatData(this, propertyName, value, formattingString)
                    ?.ToString() ?? ""));
    }

}