using System.Collections.Immutable;
using System.Text;
using CompiledTemplateEngine.SourceGenerator.Generator;
using CompiledTemplateEngine.SourceGenerator.Models;
using CompiledTemplateEngine.SourceGenerator.Parser;
using DependencyModules.SourceGenerator.Impl.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using ISourceGenerator = DependencyModules.SourceGenerator.Impl.ISourceGenerator;

namespace CompiledTemplateEngine.SourceGenerator;

public class TemplateModuleSourceGenerator : ISourceGenerator {
    private readonly TemplateParseService _templateParseService =
        new(new StringTokenNodeParser(new StringTokenNodeCreatorService()));

    public void SetupGenerator(IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<(ModuleEntryPointModel Left, DependencyModuleConfigurationModel Right)> incrementalValueProvider) {

        var provider =
            context.AdditionalTextsProvider;

        var combinedProvider =
            provider.Combine(context.AnalyzerConfigOptionsProvider).Where(FileFilter).Select(ToTemplateModel);

        context.RegisterSourceOutput(
            combinedProvider.Combine(incrementalValueProvider.Collect()),
            GenerateSource
        );
        
        context.RegisterSourceOutput(
            incrementalValueProvider.Combine(combinedProvider.Collect()),
            GenerateTemplateEntry
            );
    }

    private void GenerateTemplateEntry(SourceProductionContext context, ((ModuleEntryPointModel Left, DependencyModuleConfigurationModel Right) Left, ImmutableArray<TemplateModel> Right) templateData) {
        if (!EntryModelSelector.ModelIsAttributed(templateData.Left.Left)) {
            return;
        }
        
        var serviceGenerator = new TemplateInterfaceServiceGenerator(context.CancellationToken);
        
        serviceGenerator.GenerateInterfaceService(
            context, templateData.Left.Left, templateData.Left.Right, templateData.Right);
        
    }

    private TemplateModel ToTemplateModel((AdditionalText Left, AnalyzerConfigOptionsProvider Right) templateData, CancellationToken cancellationToken) {
        var parsed = _templateParseService.ParseTemplate(
            templateData.Left.GetText(cancellationToken)?.ToString() ?? "",
            new StringTokenNodeParser.TokenInfo("{{", "}}"));

        var extension = Path.GetExtension(templateData.Left.Path);
        var fileName = Path.GetFileNameWithoutExtension(templateData.Left.Path);

        var safeName = GetSafeName(fileName);
        
        return new TemplateModel(
            fileName,
            extension,
            GenerateClassName(parsed, templateData.Left.Path),
            safeName,
            parsed
        );
    }

    private string GetSafeName(string fileName) {
        var stringBuilder = new StringBuilder();
        
        var upperCase = true;

        foreach (var character in fileName) {
            if (upperCase) {
                stringBuilder.Append(char.ToUpperInvariant(character));
                upperCase = false;
            } else if (SkipCharacter(character)) {
                upperCase = true;
            }
            else {
                stringBuilder.Append(character);
            }
        }

        if (Char.IsDigit(fileName[0])) {
            stringBuilder.Insert(0, "N");
        }
        
        return stringBuilder.ToString();
    }

    private bool SkipCharacter(char character) {
        return !Char.IsLetterOrDigit(character);
    }

    private string GenerateClassName(IList<TemplateActionNode> parsed, string filePath) {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append(filePath);
        GenerateStringFromNodes(parsed, stringBuilder);

        using var md5 = System.Security.Cryptography.MD5.Create();
        var inputBytes = Encoding.ASCII.GetBytes(stringBuilder.ToString());
        var hashBytes = md5.ComputeHash(inputBytes);

        var hashString = Convert.ToBase64String(hashBytes).Replace("+","Pl").Replace("/","Sl").Replace("=", "Eq");
        return "Template" + hashString;
    }

    private void GenerateStringFromNodes(IList<TemplateActionNode> parsed, StringBuilder builder) {
        foreach (var templateActionNode in parsed) {
            builder.Append(templateActionNode.ActionText + templateActionNode.FieldName);

            GenerateStringFromNodes(templateActionNode.ArgumentList, builder);
            GenerateStringFromNodes(templateActionNode.ChildNodes, builder);
        }
    }

    private bool FileFilter((AdditionalText Left, AnalyzerConfigOptionsProvider Right) filterData) {
        if (filterData.Right.GetOptions(filterData.Left).TryGetValue("build_metadata.AdditionalFiles.SkipTemplate",
                out var skipTemplate) && skipTemplate == "true") {
            return false;
        }

        if (!filterData.Right.GlobalOptions.TryGetValue("build_property.CompileTemplateExtensions", out var compileTemplateExtension)) {
            compileTemplateExtension = "html;js;css;md";
        }

        var extension = compileTemplateExtension.Split(';');

        var fileExtension = filterData.Left.Path.Split('.').Last().ToLower();

        return extension.Contains(fileExtension);
    }

    private void GenerateSource(SourceProductionContext context, 
        (TemplateModel Left,
            ImmutableArray<(ModuleEntryPointModel Left, DependencyModuleConfigurationModel Right)> Right) arg2) {
        var generator = new TemplateFileGenerator();

        generator.GenerateFile(context, arg2.Left, arg2.Right);
    }
}