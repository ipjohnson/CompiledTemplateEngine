namespace CompiledTemplateEngine.Runtime.Interfaces;

public class MissingTemplateException(string templateName) :
    Exception($"Could not find template with name '{templateName}'") {
    public string TemplateName { get; } = templateName;
}