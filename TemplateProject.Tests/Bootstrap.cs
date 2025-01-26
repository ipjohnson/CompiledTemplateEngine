using DependencyModules.Testing.Attributes;
using DependencyModules.Testing.NSubstitute;
using TemplateProject;

[assembly: LoadModules(typeof(Templates))]
[assembly: NSubstituteSupport]