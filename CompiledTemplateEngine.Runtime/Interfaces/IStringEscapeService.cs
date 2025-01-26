namespace CompiledTemplateEngine.Runtime.Interfaces;

public interface IStringEscapeService {
    bool CanEscapeTemplate(string templateExtension);

    string EscapeString(string? value);
}