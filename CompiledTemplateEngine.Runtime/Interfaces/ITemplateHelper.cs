namespace CompiledTemplateEngine.Runtime.Interfaces;

public interface ITemplateHelper {
    ValueTask<object?> Execute(ITemplateExecutionContext handlerDataContext, params object[] arguments);
}