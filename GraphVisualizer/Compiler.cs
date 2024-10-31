using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using GraphLibrary;
using Microsoft.CodeAnalysis.Text;
using System.Reflection;
using GraphVisualizer.Services;
using System.IO;

namespace GraphVisualizer
{
    public class Compiler
    {
        List<MetadataReference> assembles = new List<MetadataReference>();
        CSharpCompilationOptions compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,concurrentBuild: false,optimizationLevel: OptimizationLevel.Release);
        CSharpParseOptions options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp11).WithKind(SourceCodeKind.Regular);
        ScriptLoaderService scriptLoaderService;
        public Compiler(ScriptLoaderService ScriptLoader)
        {
            scriptLoaderService = ScriptLoader;
            Setting();
        }

        async void Setting()
        {
            compilationOptions=compilationOptions.WithNullableContextOptions(NullableContextOptions.Disable);
            var assemblyNames = Assembly.GetEntryAssembly()!.GetReferencedAssemblies();

            foreach (var name in assemblyNames)
            {
                assembles.Add(await scriptLoaderService.GetAssemblyMetadataReference(Assembly.Load(name)));
            }

            assembles.Add(await scriptLoaderService.GetAssemblyMetadataReference(typeof(int).Assembly));

        }

        public string[] Compile(string code, out Assembly? assembly)
        {
            using MemoryStream stream = new MemoryStream();
            var codeString = SourceText.From(code);
            var syntaxTree = SyntaxFactory.ParseSyntaxTree(codeString, options);
            var compilation = CSharpCompilation.Create("run.dll", new[] { syntaxTree }, assembles, compilationOptions);
            var result = compilation.Emit(stream);
            if (result.Success)
            {
                stream.Seek(0, SeekOrigin.Begin);
                var memoryData = stream.ToArray();
                assembly = Assembly.Load(memoryData);
                return new string[0];
            }
            else
            {
                assembly = null;

                return result.Diagnostics.Where(x=>x.IsWarningAsError ==false).Select(x=>$"{ x.Location.GetLineSpan().StartLinePosition}-{x.Location.GetLineSpan().EndLinePosition}:{ x.GetMessage()}").ToArray();
            }
        }
        public static string BaseCode { get; } = @"using System;
using GraphLibrary;

public class ActionAlgorithm:GraphAction
{
    public override Node? Action(Node node)
    {
        // ここにアルゴリズムを記載する。
    }
}
";


        public static MethodInfo? RunMethodGet(Type baseType, Assembly assembly, string methodName)
        {

            var types = assembly.GetTypes().Where(x => x.IsSubclassOf(baseType));
            if (types.Count() > 0)
            {
                var type = types.First();
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(x => x.Name == methodName);
                methods = methods.Where(x => x.GetParameters().Length == 1);
                return methods.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
    }

    
}
