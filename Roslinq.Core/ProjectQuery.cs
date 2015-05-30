namespace Roslinq.Core
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.MSBuild;

    public class ProjectQuery
    {
        private readonly Project project;
        private ClassQuery classes;

        public ProjectQuery(string projectPath)
        {
            var workspace = MSBuildWorkspace.Create();
            this.project = workspace.OpenProjectAsync(projectPath).Result;
        }

        public ClassQuery Classes
        {
            get
            {
                if (this.classes == null)
                {
                    this.classes = new ClassQuery(this.project);
                }

                return this.classes;
            }
        }

        public ReferencesQuery References
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}