namespace Roslinq.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.MSBuild;

    public class ProjectQuery
    {
        private readonly Project project;
        private IList<ClassQuery> classes;

        public ProjectQuery(string projectPath)
        {
            var workspace = MSBuildWorkspace.Create();
            this.project = workspace.OpenProjectAsync(projectPath).Result;
        }

        public ProjectQuery Classes()
        {
            if (this.classes == null)
            {
                this.classes = GetClasses().ToList();
            }

            return this;
        }

        public ProjectQuery InheritingFrom(Type type)
        {
            if (this.classes == null)
            {
                this.classes = GetClasses().ToList();
            }

            var result = new List<ClassQuery>();
            foreach (var @class in this.classes)
            {
                if (@class.InheritsFrom(type))
                {
                    result.Add(@class);
                }
            }

            this.classes = result;
            return this;
        }

        public IEnumerable<string> Execute()
        {
            if (this.classes == null)
            {
                this.classes = GetClasses().ToList();
            }

            return this.classes.Select(c => c.ClassName);
        }

        private IEnumerable<ClassQuery> GetClasses()
        {
            foreach (var namedTypeSymbol in GetClassSymbols())
            {
                yield return new ClassQuery(namedTypeSymbol);
            }
        } 

        private IEnumerable<INamedTypeSymbol> GetClassSymbols()
        {
            this.classes = new List<ClassQuery>();
            foreach (var document in project.Documents)
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