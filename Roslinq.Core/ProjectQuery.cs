namespace Roslinq.Core
{
    using Microsoft.CodeAnalysis;
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
                return new ClassQuery(project);
            }
        }
    }
}