using System.Collections.Immutable;

namespace CompiledTemplateEngine.Runtime.Interfaces;

public interface ITemplateExecutionContext {
    ITemplateOutputWriter Writer { get; }

    ITemplateExecutionService ExecutionService { get; }
    
    ImmutableDictionary<string, object> Variables { get; }
    
    void SetVariable(string name, object value);

    IInternalTemplateServices TemplateServices { get; }

    IStringEscapeService StringEscapeService { get; }

    string TemplateExtension { get; }

    IServiceProvider RequestServiceProvider { get; }

    ITemplateExecutionContext? ParentContext { get; }

    object? ObjectValue { get; }

    SafeString GetEscapedString(object value, string propertyName = "", string formattingString = "");
}