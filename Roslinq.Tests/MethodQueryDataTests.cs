namespace Roslinq.Tests
{
    using System;
    using System.Linq;
    using Core;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using NUnit.Framework;

    [TestFixture]
    public class MethodQueryDataTests
    {
        [Test]
        public void MethodExecuteTest()
        {
            string sourceCode = @"class Bar { public void Foo() {} }";

            var mscorlib = MetadataReference.CreateFromAssembly(typeof(object).Assembly);
            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            var compilation = CSharpCompilation.Create("TestAsm", new[] { syntaxTree }, new[] { mscorlib }, compilationOptions);

            var semanticModel = compilation.GetSemanticModel(syntaxTree, false);
            var fooMethodSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().ToList()[0];
            var fooMethodSymbol = semanticModel.GetDeclaredSymbol(fooMethodSyntax);
            var methodSymbolInfo = new MethodQueryData(fooMethodSymbol);
            Assert.AreEqual("Foo", methodSymbolInfo.MethodName);
        }

        [Test]
        public void MethodReturningTypeTest()
        {
            string sourceCode = @"class Bar { public int Foo() { return 0; } }";

            var mscorlib = MetadataReference.CreateFromAssembly(typeof(object).Assembly);
            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            var compilation = CSharpCompilation.Create("TestAsm", new[] { syntaxTree }, new[] { mscorlib }, compilationOptions);

            var semanticModel = compilation.GetSemanticModel(syntaxTree, false);
            var fooMethodSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().ToList()[0];
            var fooMethodSymbol = semanticModel.GetDeclaredSymbol(fooMethodSyntax);
            var methodSymbolInfo = new MethodQueryData(fooMethodSymbol);
            Assert.True(methodSymbolInfo.ReturnsType(typeof(int)));
        }

        [Test]
        public void MethodReturningVoidTest()
        {
            string sourceCode = @"class Bar { public void Foo() { } }";

            var mscorlib = MetadataReference.CreateFromAssembly(typeof(object).Assembly);
            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            var compilation = CSharpCompilation.Create("TestAsm", new[] { syntaxTree }, new[] { mscorlib }, compilationOptions);

            var semanticModel = compilation.GetSemanticModel(syntaxTree, false);
            var fooMethodSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().ToList()[0];
            var fooMethodSymbol = semanticModel.GetDeclaredSymbol(fooMethodSyntax);
            var methodSymbolInfo = new MethodQueryData(fooMethodSymbol);
            Assert.True(methodSymbolInfo.ReturnsType(typeof(void)));
        }

        [Test]
        public void HasAttributeAppliedTest()
        {
            string sourceCode = @"using System; class Bar { [Obsolete] public void Foo() { } }";

            var mscorlib = MetadataReference.CreateFromAssembly(typeof(object).Assembly);
            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            var compilation = CSharpCompilation.Create("TestAsm", new[] { syntaxTree }, new[] { mscorlib }, compilationOptions);
            if (compilation.GetDeclarationDiagnostics().Any())
            {
                Assert.Fail("compile errors");
            }

            var semanticModel = compilation.GetSemanticModel(syntaxTree, false);
            var fooMethodSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().ToList()[0];
            var fooMethodSymbol = semanticModel.GetDeclaredSymbol(fooMethodSyntax);
            var methodSymbolInfo = new MethodQueryData(fooMethodSymbol);
            Assert.True(methodSymbolInfo.HasAttributeApplied(typeof(ObsoleteAttribute)));
        }
    }
}