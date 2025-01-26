namespace CompiledTemplateEngine.Runtime.Interfaces;

public interface IStringEscapeServiceProvider {
    IStringEscapeService GetEscapeService(string templateExtension);
}