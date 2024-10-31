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
        CSharpParseOptions options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp8).WithKind(SourceCodeKind.Regular);
        ScriptLoaderService scriptLoaderService;
        public Compiler(ScriptLoaderService ScriptLoader)
        {
            scriptLoaderService = ScriptLoader;
            Task.Run(async () =>
            {
                assembles.Add(await ScriptLoader.GetAssemblyMetadataReference(typeof(int).Assembly));
                assembles.Add(await ScriptLoader.GetAssemblyMetadataReference(typeof(GraphLibrary.Node).Assembly));

            });
            
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

                return result.Diagnostics.Select(x=>$"{ x.Location}:{ x.GetMessage()}").ToArray();
            }
        }
    }
}
