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

        internal ClassQuery(Project parentProject)
        {
            this.parentProject = parentProject;
        }

        /// <summary>
        /// Entry point for creating and executing method queries.
        /// </summary>
        public MethodQuery Methods
        {
            get
            {
                return new MethodQuery(this);
            }
        }

        /// <summary>
        /// Executes the query with all previously applied filters.
        /// </summary>
        /// <returns>The list of <see cref="ClassQueryData"/></returns>
        public IList<ClassQueryData> Execute()
        {
            EnsureClassesExist();
            return this.classes;
        }

        /// <summary>
        /// Filter classes by type they inherit from (directly or indirectly).
        /// </summary>
        /// <param name="type">The type that class inherits from.</param>
        public ClassQuery InheritingFrom(Type type)
        {
            EnsureClassesExist();
            this.Filter(x => x.InheritsFrom(type));
            return this;
        }

        /// <summary>
        /// Filter classes by interface they implement.
        /// </summary>
        /// <param name="interfaceName">Name of the interface.</param>
        public ClassQuery ImplementingInterface(string interfaceName)
        {
            EnsureClassesExist();
            this.Filter(x => x.ImplementsInterface(interfaceName));
            return this;
        }

        /// <summary>
        /// Filter classes by attribute thay have applied.
        /// </summary>
        /// <param name="type">Attribute type.</param>
        /// <exception cref="ArgumentException">If the type is not attribute type.</exception>
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

        /// <summary>
        /// Filter classes by modifier they have applied.
        /// </summary>
        /// <param name="modifier"><see cref="Modifiers.Class"/> access modifier.</param>
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