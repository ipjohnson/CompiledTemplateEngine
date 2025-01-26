namespace CompiledTemplateEngine.SourceGenerator.Models;

public class TemplateHelperModel {
    
}

public class TemplateHelperComparer : IEqualityComparer<TemplateHelperModel> {
    public bool Equals(TemplateHelperModel? x, TemplateHelperModel? y) {

        return false;
    }

    public int GetHashCode(TemplateHelperModel obj) {
        return 0;
    }
}