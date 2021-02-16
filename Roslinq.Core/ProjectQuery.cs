namespace Roslinq
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Build.Locator;
    using Microsoft.CodeAnalysis.MSBuild;

    public class ProjectQuery
    {
        private readonly IList<ProjectQueryData> projects;

        internal ProjectQuery(IList<ProjectQueryData> projects)
        {
            this.projects = projects;
        }

        /// <summary>
        /// Creates new instance of ProjectQuery based on a project stored in given location on disk.
        /// </summary>
        /// <param name="projectPath">Full path to project location on disk.</param>
        public ProjectQuery(string projectPath)
        {
            MSBuildLocator.RegisterDefaults();
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            this.projects = new List<ProjectQueryData> {new ProjectQueryData(workspace.OpenProjectAsync(projectPath).Result)};
        }

        /// <summary>
        /// Executes the query with all previously applied filters.
        /// </summary>
        /// <returns>The list of <see cref="ProjectQueryData"/></returns>
        public IList<ProjectQueryData> Execute()
        {
            return this.projects;
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

            foreach (var p in this.projects)
            {
                foreach (var namedTypeSymbol in p.Classes)
                {
                    yield return new ClassQueryData(namedTypeSymbol);
                }
            }
        }
    }
}