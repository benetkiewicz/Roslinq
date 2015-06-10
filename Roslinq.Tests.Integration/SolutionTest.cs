namespace Roslinq.Tests.Integration
{
    using Core;
    using NUnit.Framework;

    public class SolutionTest
    {
        [Test]
        public void ManySolutionClassesShouldBePresentTest()
        {
            var codeQuery = new SolutionQuery(@"..\..\..\Roslinq.sln");
            var classes = codeQuery.Classes.Execute();
            Assert.IsNotNull(classes);
            Assert.IsTrue(classes.Count > 2);
        }

        [Test]
        public void ManyProjectSolutionClassesShouldBePresent()
        {
            var codeQuery = new SolutionQuery(@"..\..\..\Roslinq.sln");
            var classes = codeQuery.Projects.Classes.Execute();
            Assert.IsNotNull(classes);
            Assert.IsTrue(classes.Count > 2);
        }

        [Test]
        public void ClassesFromDirectSolutionAndClassesFromSolutionProjectsShouldMatch()
        {
            var solutionQuery1 = new SolutionQuery(@"..\..\..\Roslinq.sln");
            var projectsClasses = solutionQuery1.Projects.Classes.Execute();

            var solutionQuery2 = new SolutionQuery(@"..\..\..\Roslinq.sln");
            var solutionClasses = solutionQuery2.Classes.Execute();

            Assert.AreEqual(projectsClasses.Count, solutionClasses.Count);
        }
    }
}