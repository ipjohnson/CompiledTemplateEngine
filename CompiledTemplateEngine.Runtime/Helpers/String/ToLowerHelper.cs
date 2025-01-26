namespace CompiledTemplateEngine.Runtime.Helpers.String;

public class ToLowerHelper : BaseStringHelper {
    protected override object AugmentString(string value) {
        return value.ToLowerInvariant();
    }
}