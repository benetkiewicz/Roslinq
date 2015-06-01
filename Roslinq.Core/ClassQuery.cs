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
        private IList<ClassQueryData> classes;

        public ClassQuery(Project parentProject)
        {
            this.parentProject = parentProject;
        }

        public MethodQuery Methods
        {
            get
            {
                return new MethodQuery(this);
            }
        }

        public IList<ClassQueryData> Execute()
        {
            EnsureClassesExist();
            return this.classes;
        }

        public ClassQuery InheritingFrom(Type type)
        {
            EnsureClassesExist();
            this.Filter(x => x.InheritsFrom(type));
            return this;
        }

        public ClassQuery ImplementingInterface(string interfaceName)
        {
            EnsureClassesExist();
            this.Filter(x => x.ImplementsInterface(interfaceName));
            return this;
        }

        public ClassQuery WithAttribute(Type type)
        {
            if (!InheritanceHelper.InheritsFrom(type, typeof(Attribute)))
            {
                throw new ArgumentException("Only attribute types allowed", "type");
            }

            EnsureClassesExist();
            this.Filter(x => x.HasAttributeApplied(type));
            return this;
        }

        public ClassQuery WithModifier(int modifier)
        {
            EnsureClassesExist();
            this.Filter(x => x.HasModifier(modifier));
            return this;
        }

        private void Filter(Func<ClassQueryData, bool> predicate)
        {
            var result = this.classes.Where(predicate).ToList();
            this.classes = result;
        }

        private void EnsureClassesExist()
        {
            if (this.classes == null)
            {
                this.classes = GetClasses().ToList();
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
    }
}