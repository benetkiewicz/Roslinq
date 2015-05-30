namespace Roslinq.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class ClassQuery
    {
        private readonly Project parentProject;

        public ClassQuery(Project parentProject)
        {
            this.parentProject = parentProject;
        }

        private IList<ClassQueryData> classes;

        public ClassQuery InheritingFrom(Type type)
        {
            if (this.classes == null)
            {
                this.classes = GetClasses().ToList();
            }

            var result = new List<ClassQueryData>();
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

        private IEnumerable<ClassQueryData> GetClasses()
        {
            foreach (var namedTypeSymbol in GetClassSymbols())
            {
                yield return new ClassQueryData(namedTypeSymbol);
            }
        }

        private IEnumerable<INamedTypeSymbol> GetClassSymbols()
        {
            this.classes = new List<ClassQueryData>();
            foreach (var document in this.parentProject.Documents)
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

        public ClassQuery ImplementingInterface(string icontroller)
        {
            if (this.classes == null)
            {
                this.classes = GetClasses().ToList();
            }

            IList<ClassQueryData> result = new List<ClassQueryData>();
            foreach (var @class in this.classes)
            {
                if (@class.ImplementsInterface(icontroller))
                {
                    result.Add(@class);
                }
            }

            this.classes = result;
            return this;
        }
    }
}