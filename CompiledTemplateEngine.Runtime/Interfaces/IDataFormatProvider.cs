namespace CompiledTemplateEngine.Runtime.Interfaces;

public interface IDataFormatProvider {
    void ProvideFormatters(IDictionary<Type, FormatDataFunc> formatter);
}