namespace Roslinq
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class ProjectQueryData
    {
        private readonly Project project;

        public ProjectQueryData(Project project)
        {
            this.project = project;
        }

        public string ProjectName
        {
            get { return this.project.Name; }
        }

        internal IEnumerable<INamedTypeSymbol> Classes
        {
            get
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
}