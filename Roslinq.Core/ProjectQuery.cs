﻿namespace Roslinq.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.MSBuild;

    public class ProjectQuery
    {
        private readonly Project project;

        public ProjectQuery(string projectPath)
        {
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            this.project = workspace.OpenProjectAsync(projectPath).Result;
        }

        /// <summary>
        /// Entry point for creating and executing class queries.
        /// </summary>
        public ClassQuery Classes
        {
            get
            {
                return new ClassQuery(GetClasses().ToList());
            }
        }

        private IEnumerable<ClassQueryData> GetClasses()
        {
            foreach (var namedTypeSymbol in GetClassSymbols())
            {
                yield return new ClassQueryData(namedTypeSymbol);
            }
        }

        private IEnumerable<INamedTypeSymbol> GetClassSymbols()
        {
            foreach (var document in this.project.Documents)
            {
                var model = document.GetSemanticModelAsync().Result;
                var syntaxTree = model.SyntaxTree;
                var classSyntaxNodes = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();
                foreach (var classSyntaxNode in classSyntaxNodes)
                {
                    var classSymbol = (INamedTypeSymbol)model.GetDeclaredSymbol(classSyntaxNode);
                    yield return classSymbol;
                }
            }
        }
    }
}