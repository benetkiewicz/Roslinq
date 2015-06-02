﻿using System;
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
    public class TypeComparisonTests
    {
        [Test]
        public void SameTypesShouldMatch()
        {
            string sourceCode = @"class Bar : object {}";

            var mscorlib = MetadataReference.CreateFromAssembly(typeof(object).Assembly);
            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            var compilation = CSharpCompilation.Create("TestAsm", new[] { syntaxTree }, new[] { mscorlib }, compilationOptions);

            var semanticModel = compilation.GetSemanticModel(syntaxTree, false);
            var barClassSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().ToList()[0];
            var barClassSymbolInfo = semanticModel.GetDeclaredSymbol(barClassSyntax);
            var classBarBaseTypeSymbolInfo = barClassSymbolInfo.BaseType;

            Assert.True(TypeComparer.TypesMatch(classBarBaseTypeSymbolInfo, typeof(object)));
        }

        [Test]
        public void TypesWithSameNameOnlyShouldNotMatch()
        {
            string sourceCode = @"public interface ICloneable { }";

            var mscorlib = MetadataReference.CreateFromAssembly(typeof(object).Assembly);
            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
            var compilation = CSharpCompilation.Create("TestAsm", new[] { syntaxTree }, new[] { mscorlib }, compilationOptions);

            if (compilation.GetDiagnostics().Any())
            {
                Assert.Fail("compilation failed");
            }

            var semanticModel = compilation.GetSemanticModel(syntaxTree, false);
            var interfaceDeclarationSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<InterfaceDeclarationSyntax>().ToList()[0];
            var interfaceSymbol = semanticModel.GetDeclaredSymbol(interfaceDeclarationSyntax);

            Assert.False(TypeComparer.TypesMatch(interfaceSymbol, typeof(ICloneable)));
        }
    }
}
