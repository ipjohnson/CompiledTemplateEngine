using System.Diagnostics.CodeAnalysis;

namespace CompiledTemplateEngine.SourceGenerator.Models;

public enum TemplateActionType {
    Content,
    Block,
    MustacheToken,
    RawMustacheToken,
    StringLiteral
}

public enum TemplateActionNodeTrimAttribute {
    OpenStart,
    OpenEnd,
    CloseStart,
    CloseEnd,
}

public class TemplateActionNode {
    
    private static readonly IList<TemplateActionNodeTrimAttribute> EmptyTrimAttributes =
        new List<TemplateActionNodeTrimAttribute>(0);

    private static readonly IList<TemplateActionNode> EmptyList = new List<TemplateActionNode>(0);

    public TemplateActionNode(
        TemplateActionType action,
        string actionText)
        : this(action, actionText, EmptyList, EmptyList, EmptyTrimAttributes) { }

    public TemplateActionNode(
        TemplateActionType action,
        string actionText,
        IList<TemplateActionNode> argumentList)
        : this(action, actionText, argumentList, EmptyList, EmptyTrimAttributes) { }

    public TemplateActionNode(
        TemplateActionType action,
        string actionText,
        IList<TemplateActionNode> argumentList,
        IList<TemplateActionNode> childNodes,
        IList<TemplateActionNodeTrimAttribute> trimAttributes) {
        Action = action;
        ActionText = actionText;
        ChildNodes = childNodes;
        ArgumentList = argumentList;
        TrimAttributes = trimAttributes;
        FieldName = "";
    }

    public TemplateActionType Action { get; }

    public string ActionText { get; set; }

    public IList<TemplateActionNode> ArgumentList { get; }

    public IList<TemplateActionNode> ChildNodes { get; }

    public IList<TemplateActionNodeTrimAttribute> TrimAttributes { get; }

    public string FieldName { get; set; }
    
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is not TemplateActionNode other) return false;

        if (Action != other.Action || ActionText != other.ActionText)
            return false;

        if (!ArgumentList.SequenceEqual(other.ArgumentList))
            return false;

        if (!ChildNodes.SequenceEqual(other.ChildNodes))
            return false;

        if (!TrimAttributes.SequenceEqual(other.TrimAttributes))
            return false;

        return true;
    }
    
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 31 + Action.GetHashCode();
        hash = hash * 31 + (ActionText?.GetHashCode() ?? 0);

        foreach (var argument in ArgumentList)
        {
            hash = hash * 31 + (argument?.GetHashCode() ?? 0);
        }
        
        foreach (var child in ChildNodes)
        {
            hash = hash * 31 + (child?.GetHashCode() ?? 0);
        }
        
        foreach (var trimAttribute in TrimAttributes)
        {
            hash = hash * 31 + trimAttribute.GetHashCode();
        }

        return hash;
    }
}