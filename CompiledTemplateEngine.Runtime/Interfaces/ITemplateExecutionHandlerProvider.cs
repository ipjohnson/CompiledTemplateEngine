namespace CompiledTemplateEngine.Runtime.Interfaces;

public interface ITemplateExecutionHandlerProvider {
    ITemplateExecutionService? TemplateExecutionService { get; set; }

    ITemplateExecutionHandler? GetTemplateExecutionHandler(string templateName);
}