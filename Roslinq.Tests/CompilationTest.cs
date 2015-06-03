namespace Roslinq.Tests
{
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using NUnit.Framework;

    public class CompilationTest
    {
        protected SemanticModel CompileAndGetSymanticModel(string sourceCode)
        {
            var mscorlib = MetadataReference.CreateFromAssembly(typeof(object).Assembly);
            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            var compilation = CSharpCompilation.Create("TestAsm", new[] { syntaxTree }, new[] { mscorlib }, compilationOptions);
            var diagnostics = compilation.GetDeclarationDiagnostics().ToList();
            if (diagnostics.Any())
            {
                Assert.Fail("compilation error: " + diagnostics[0].GetMessage());
            }

            var semanticModel = compilation.GetSemanticModel(syntaxTree, false);
            return semanticModel;
        }

    }
}