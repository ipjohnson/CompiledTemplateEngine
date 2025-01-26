using System.Web;
using CompiledTemplateEngine.Runtime.Interfaces;
using DependencyModules.Runtime.Attributes;

namespace CompiledTemplateEngine.Runtime.Engine;


[SingletonService(ServiceType = typeof(IStringEscapeService))]
public class HtmlEscapeStringService : IStringEscapeService {
    public bool CanEscapeTemplate(string templateExtension) {
        return templateExtension.EndsWith("html");
    }

    public string EscapeString(string? value) {
        if (value == null) {
            return string.Empty;
        }

        return HttpUtility.HtmlEncode(value);
    }
}