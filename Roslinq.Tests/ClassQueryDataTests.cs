using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roslinq.Tests
{
    using Core;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using NUnit.Framework;

    [TestFixture]
    public class ClassQueryDataTests
    {
        [Test]
        public void InheritsFromTest()
        {
            string sourceCode = @"class Bar : object {}";

            var mscorlib = MetadataReference.CreateFromAssembly(typeof(object).Assembly);
            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            var compilation = CSharpCompilation.Create("TestAsm", new[] { syntaxTree }, new[] { mscorlib }, compilationOptions);

            var semanticModel = compilation.GetSemanticModel(syntaxTree, false);
            var barClassSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().ToList()[0];
            var barClassSymbolInfo = semanticModel.GetDeclaredSymbol(barClassSyntax);
            var classQuery = new ClassQueryData((INamedTypeSymbol)barClassSymbolInfo);
            Assert.True(classQuery.InheritsFrom(typeof(Object)));
        }

        [Test]
        public void ImplementsInterfaceTest()
        {
            string sourceCode = @"using System; class Bar : IDisposable { public void Dispose() {} }";

            var mscorlib = MetadataReference.CreateFromAssembly(typeof(object).Assembly);
            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            var compilation = CSharpCompilation.Create("TestAsm", new[] { syntaxTree }, new[] { mscorlib }, compilationOptions);
            if (compilation.GetDeclarationDiagnostics().Any())
            {
                Assert.Fail("compile errors");
            }

            var semanticModel = compilation.GetSemanticModel(syntaxTree, false);
            var barClassSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().ToList()[0];
            var barClassSymbolInfo = semanticModel.GetDeclaredSymbol(barClassSyntax);
            var classQuery = new ClassQueryData((INamedTypeSymbol)barClassSymbolInfo);
            Assert.True(classQuery.ImplementsInterface(typeof(IDisposable)));
        }

        [Test]
        public void HasAttributeAppliedTest()
        {
            string sourceCode = @"using System; [Serializable] class Bar { }";

            var mscorlib = MetadataReference.CreateFromAssembly(typeof(object).Assembly);
            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            var compilation = CSharpCompilation.Create("TestAsm", new[] { syntaxTree }, new[] { mscorlib }, compilationOptions);
            if (compilation.GetDeclarationDiagnostics().Any())
            {
                Assert.Fail("compile errors");
            }

            var semanticModel = compilation.GetSemanticModel(syntaxTree, false);
            var barClassSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().ToList()[0];
            var barClassSymbolInfo = semanticModel.GetDeclaredSymbol(barClassSyntax);
            var classQueryData = new ClassQueryData(barClassSymbolInfo);
            Assert.IsTrue(classQueryData.HasAttributeApplied(typeof(SerializableAttribute)));
        }

        [Test]
        public void HasModifier()
        {
            string sourceCode = @"using System; class Bar { private sealed class Foo { } } ";

            var mscorlib = MetadataReference.CreateFromAssembly(typeof(object).Assembly);
            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            var compilation = CSharpCompilation.Create("TestAsm", new[] { syntaxTree }, new[] { mscorlib }, compilationOptions);
            if (compilation.GetDeclarationDiagnostics().Any())
            {
                Assert.Fail("compile errors: " + compilation.GetDeclarationDiagnostics()[0].GetMessage());
            }

            var semanticModel = compilation.GetSemanticModel(syntaxTree, false);
            var fooClassSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().ToList()[1];
            var fooClassSymbol = semanticModel.GetDeclaredSymbol(fooClassSyntax);
            var classQueryData = new ClassQueryData(fooClassSymbol);
            Assert.IsTrue(classQueryData.HasModifier(Modifiers.Class.Private));
            Assert.IsFalse(classQueryData.HasModifier(Modifiers.Class.Protected));
        }
    }
}
