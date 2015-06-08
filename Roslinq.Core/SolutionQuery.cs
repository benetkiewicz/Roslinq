namespace Roslinq.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.MSBuild;

    public class SolutionQuery
    {
        private readonly Solution solution;
        public SolutionQuery(string solutionPath)
        {
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            this.solution = workspace.OpenSolutionAsync(solutionPath).Result;
        }

        /// <summary>
        /// Entry point for creating and executing class queries.
        /// </summary>
        /// <remarks>Queries over all project classes that are part of the solution.</remarks>
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
            foreach (var project in this.solution.Projects)
            {
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
}