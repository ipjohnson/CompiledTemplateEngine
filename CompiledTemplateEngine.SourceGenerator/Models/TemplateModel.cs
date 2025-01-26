namespace CompiledTemplateEngine.SourceGenerator.Models;


public class TemplateModel {
    public TemplateModel(
        string templateName,
        string templateExtension,
        string templateClassName,
        string safeName,
        IList<TemplateActionNode> templateActionNodes) {
        TemplateName = templateName;
        TemplateClassName = templateClassName;
        SafeName = safeName;
        TemplateExtension = templateExtension;
        TemplateActionNodes = templateActionNodes;
    }

    public IList<TemplateActionNode> TemplateActionNodes { get; }
    
    public string TemplateName { get; }

    public string TemplateExtension { get; }
    
    public string TemplateClassName { get; }

    public string SafeName {
        get;
    }
}

public class TemplateModelComparer : IEqualityComparer<TemplateModel> {
    
    public bool Equals(TemplateModel? x, TemplateModel? y) {
        if (x is null && y is null)
            return true;

        if (x is null || y is null)
            return false;

        return 
            x.TemplateName == y.TemplateName &&
               x.SafeName == y.SafeName &&
               x.TemplateExtension == y.TemplateExtension &&
               x.TemplateActionNodes.SequenceEqual(y.TemplateActionNodes);
    }

    public int GetHashCode(TemplateModel obj) {
        if (obj is null) throw new ArgumentNullException(nameof(obj));

        int hashCode = 17;
        hashCode = hashCode * 31 + obj.TemplateName.GetHashCode();
        hashCode = hashCode * 31 + obj.SafeName.GetHashCode();
        hashCode = hashCode * 31 + obj.TemplateExtension.GetHashCode();

        foreach (var actionNode in obj.TemplateActionNodes) {
            hashCode = hashCode * 31 + (actionNode?.GetHashCode() ?? 0);
        }

        return hashCode;
    }
}

