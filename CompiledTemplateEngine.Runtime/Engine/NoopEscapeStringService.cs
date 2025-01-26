using CompiledTemplateEngine.Runtime.Interfaces;
using DependencyModules.Runtime.Attributes;

namespace CompiledTemplateEngine.Runtime.Engine;

[SingletonService(ServiceType = typeof(IStringEscapeService))]
public class NoopEscapeStringService : IStringEscapeService {
    public bool CanEscapeTemplate(string templateExtension) {
        return templateExtension.EndsWith("css") ||
               templateExtension.EndsWith("js") ||
               templateExtension.EndsWith("md");
    }

    public string EscapeString(string? value) {
        return value ?? "";
    }
}