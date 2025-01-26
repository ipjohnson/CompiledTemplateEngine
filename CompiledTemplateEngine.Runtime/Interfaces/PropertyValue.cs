namespace CompiledTemplateEngine.Runtime.Interfaces;

public interface IPropertyValue {
    string Name { get; }

    Type PropertyType { get; }

    object Value { get; }
}

public class PropertyValue {
    public static PropertyValue<T> From<T>(T value, string name) {
        return new PropertyValue<T>(value, name);
    }
}

public class PropertyValue<T>(T value, string name) : IPropertyValue {

    public object Value { get; } = value!;

    public string Name { get; } = name;

    public Type PropertyType => typeof(T);

    public override string ToString() {
        return Value.ToString() ?? "";
    }
}