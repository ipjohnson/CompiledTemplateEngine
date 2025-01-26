namespace TemplateProject.Models;

public record SimpleOutputModel(string StringValue, int IntValue, List<string> StringList, IDictionary<string, int> IntDictionary);