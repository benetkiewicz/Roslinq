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
        private IList<INamedTypeSymbol> classes;

        public ProjectQuery(string projectPath)
        {
            var workspace = MSBuildWorkspace.Create();
            this.project = workspace.OpenProjectAsync(projectPath).Result;
        }

        public IEnumerable<INamedTypeSymbol> Classes
        {
            get
            {
                if (this.classes == null)
                {
                    this.classes = new List<INamedTypeSymbol>();
                    foreach (var document in project.Documents)
                    {
                        var model = document.GetSemanticModelAsync().Result;
                        var syntaxTree = model.SyntaxTree;
                        var classSyntaxNodes = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();
                        foreach (var classSyntaxNode in classSyntaxNodes)
                        {
                            var classSymbol = (INamedTypeSymbol)model.GetDeclaredSymbol(classSyntaxNode);
                            this.classes.Add(classSymbol);
                        }
                    }
                }

                return this.classes;
            }
        }

        public IEnumerable<string> ClassesInheritingFrom(Type type)
        {
            foreach (var @class in Classes)
            {
                if (ProjectQuery.DirectlyOrIndirectlyInheritsFrom(@class, type.ToString()))
                {
                    yield return @class.Name;
                }
            }
        }

        public static bool DirectlyOrIndirectlyInheritsFrom(INamedTypeSymbol queriedType, string typeName)
        {
            var classBaseType = queriedType.BaseType;
            if (classBaseType.ToString() == typeName)
            {
                return true;
            }

            if (classBaseType.ToString() == "object")
            {
                return false;
            }

            return DirectlyOrIndirectlyInheritsFrom(classBaseType, typeName);
        }
    }

}