namespace Roslinq.Tests
{
    using System;
    using System.Collections.Immutable;
    using System.Linq;
    using Core;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using NUnit.Framework;

    [TestFixture]
    public class MethodQueryDataTests : CompilationTest
    {
        [Test]
        public void MethodExecuteTest()
        {
            string sourceCode = @"class Bar { public void Foo() {} }";
            var semanticModel = CompileAndGetSymanticModel(sourceCode);
            var syntaxTree = semanticModel.SyntaxTree;
            var fooMethodSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().ToList()[0];
            var fooMethodSymbol = semanticModel.GetDeclaredSymbol(fooMethodSyntax);
            var methodSymbolInfo = new MethodQueryData(fooMethodSymbol);
            Assert.AreEqual("Foo", methodSymbolInfo.MethodName);
        }

        [Test]
        public void MethodReturningTypeTest()
        {
            string sourceCode = @"class Bar { public int Foo() { return 0; } }";

            var semanticModel = CompileAndGetSymanticModel(sourceCode);
            var syntaxTree = semanticModel.SyntaxTree;
            var fooMethodSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().ToList()[0];
            var fooMethodSymbol = semanticModel.GetDeclaredSymbol(fooMethodSyntax);
            var methodSymbolInfo = new MethodQueryData(fooMethodSymbol);
            Assert.True(methodSymbolInfo.ReturnsType(typeof(int)));
        }

        [Test]
        public void MethodReturningVoidTest()
        {
            string sourceCode = @"class Bar { public void Foo() { } }";
            var semanticModel = CompileAndGetSymanticModel(sourceCode);
            var syntaxTree = semanticModel.SyntaxTree;
            var fooMethodSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().ToList()[0];
            var fooMethodSymbol = semanticModel.GetDeclaredSymbol(fooMethodSyntax);
            var methodSymbolInfo = new MethodQueryData(fooMethodSymbol);
            Assert.True(methodSymbolInfo.ReturnsType(typeof(void)));
        }

        [Test]
        public void HasAttributeAppliedTest()
        {
            string sourceCode = @"using System; class Bar { [Obsolete] public void Foo() { } }";
            var semanticModel = CompileAndGetSymanticModel(sourceCode);
            var syntaxTree = semanticModel.SyntaxTree;
            var fooMethodSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().ToList()[0];
            var fooMethodSymbol = semanticModel.GetDeclaredSymbol(fooMethodSyntax);
            var methodSymbolInfo = new MethodQueryData(fooMethodSymbol);
            Assert.True(methodSymbolInfo.HasAttributeApplied(typeof(ObsoleteAttribute)));
        }

        [Test]
        public void HasParameterTypeTest()
        {
            string sourceCode = @"using System; class Bar { public int Foo(int a) { return a+1; } }";
            var semanticModel = CompileAndGetSymanticModel(sourceCode);
            var syntaxTree = semanticModel.SyntaxTree;
            var fooMethodSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().ToList()[0];
            var fooMethodSymbol = semanticModel.GetDeclaredSymbol(fooMethodSyntax);
            var methodSymbolInfo = new MethodQueryData(fooMethodSymbol);
            Assert.True(methodSymbolInfo.HasParameterType(typeof(int)));
            Assert.False(methodSymbolInfo.HasParameterType(typeof(string)));
        }

        [Test]
        public void HasParameterTypeParamsTest()
        {
            string sourceCode = @"using System; class Bar { public int Foo(params int[] a) { return 0; } }";
            var semanticModel = CompileAndGetSymanticModel(sourceCode);
            var syntaxTree = semanticModel.SyntaxTree;
            var fooMethodSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().ToList()[0];
            var fooMethodSymbol = semanticModel.GetDeclaredSymbol(fooMethodSyntax);
            var methodSymbolInfo = new MethodQueryData(fooMethodSymbol);
            Assert.True(methodSymbolInfo.HasParameterType(typeof(int)));
            Assert.False(methodSymbolInfo.HasParameterType(typeof(string)));
        }

        [Test]
        public void HasModifierTest()
        {
            string sourceCode = @"using System; class Bar { public virtual int Foo(params int[] a) { return 0; } }";
            var semanticModel = CompileAndGetSymanticModel(sourceCode);
            var syntaxTree = semanticModel.SyntaxTree;
            var fooMethodSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().ToList()[0];
            var fooMethodSymbol = semanticModel.GetDeclaredSymbol(fooMethodSyntax);
            var methodSymbolInfo = new MethodQueryData(fooMethodSymbol);
            Assert.IsTrue(methodSymbolInfo.HasModifier(Modifiers.Method.Virtual));
            Assert.IsFalse(methodSymbolInfo.HasModifier(Modifiers.Method.Protected));
        }
    }
}