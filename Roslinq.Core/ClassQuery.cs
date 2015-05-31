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

        public MethodQuery Methods
        {
            get
            {
                return new MethodQuery(this);
            }
        }

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

        public IEnumerable<ClassQueryData> Execute()
        {
            if (this.classes == null)
            {
                this.classes = GetClasses().ToList();
            }

            return this.classes;
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

        public ClassQuery ImplementingInterface(string interfaceName)
        {
            if (this.classes == null)
            {
                this.classes = GetClasses().ToList();
            }

            IList<ClassQueryData> result = new List<ClassQueryData>();
            foreach (var @class in this.classes)
            {
                if (@class.ImplementsInterface(interfaceName))
                {
                    result.Add(@class);
                }
            }

            this.classes = result;
            return this;
        }

        public ClassQuery WithAttribute(Type type)
        {
            //if (!(type is Attribute))
            //{
            //    throw new ArgumentException("Only attribute types allowed", "type");
            //}

            if (this.classes == null)
            {
                this.classes = GetClasses().ToList();
            }

            IList<ClassQueryData> result = new List<ClassQueryData>();
            foreach (var @class in this.classes)
            {
                if (@class.HasAttributeApplied(type))
                {
                    result.Add(@class);
                }
            }

            this.classes = result;
            return this;
        }

        public ClassQuery WithModifier(int modifier)
        {
            //if (!(type is Attribute))
            //{
            //    throw new ArgumentException("Only attribute types allowed", "type");
            //}

            if (this.classes == null)
            {
                this.classes = GetClasses().ToList();
            }

            IList<ClassQueryData> result = new List<ClassQueryData>();
            foreach (var @class in this.classes)
            {
                if (@class.HasModifier(modifier))
                {
                    result.Add(@class);
                }
            }

            this.classes = result;
            return this;
        }
    }
}