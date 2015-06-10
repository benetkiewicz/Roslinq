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
    public class ClassQueryDataTests : CompilationTest
    {
        [Test]
        public void InheritsFromTest()
        {
            string sourceCode = @"class Bar : object {}";
            var semanticModel = CompileAndGetSymanticModel(sourceCode);
            var syntaxTree = semanticModel.SyntaxTree;
            var barClassSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().ToList()[0];
            var barClassSymbolInfo = semanticModel.GetDeclaredSymbol(barClassSyntax);
            var classQuery = new ClassQueryData((INamedTypeSymbol)barClassSymbolInfo);
            Assert.True(classQuery.InheritsFrom(typeof(Object)));
        }

        [Test]
        public void ImplementsInterfaceTest()
        {
            string sourceCode = @"using System; class Bar : IDisposable { public void Dispose() {} }";
            var semanticModel = CompileAndGetSymanticModel(sourceCode);
            var syntaxTree = semanticModel.SyntaxTree;
            var barClassSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().ToList()[0];
            var barClassSymbolInfo = semanticModel.GetDeclaredSymbol(barClassSyntax);
            var classQuery = new ClassQueryData((INamedTypeSymbol)barClassSymbolInfo);
            Assert.True(classQuery.ImplementsInterface(typeof(IDisposable)));
        }

        [Test]
        public void HasAttributeAppliedTest()
        {
            string sourceCode = @"using System; [Serializable] class Bar { }";
            var semanticModel = CompileAndGetSymanticModel(sourceCode);
            var syntaxTree = semanticModel.SyntaxTree;
            var barClassSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().ToList()[0];
            var barClassSymbolInfo = semanticModel.GetDeclaredSymbol(barClassSyntax);
            var classQueryData = new ClassQueryData(barClassSymbolInfo);
            Assert.IsTrue(classQueryData.HasAttributeApplied(typeof(SerializableAttribute)));
        }

        [Test]
        public void HasModifier()
        {
            string sourceCode = @"using System; class Bar { private sealed class Foo { } } ";
            var semanticModel = CompileAndGetSymanticModel(sourceCode);
            var syntaxTree = semanticModel.SyntaxTree;
            var fooClassSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().ToList()[1];
            var fooClassSymbol = semanticModel.GetDeclaredSymbol(fooClassSyntax);
            var classQueryData = new ClassQueryData(fooClassSymbol);
            Assert.IsTrue(classQueryData.HasModifier(ClassModifier.Private));
            Assert.IsFalse(classQueryData.HasModifier(ClassModifier.Protected));
        }
    }
}
