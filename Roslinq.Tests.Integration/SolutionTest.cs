namespace Roslinq.Tests.Integration
{
    using Core;
    using NUnit.Framework;

    public class SolutionTest
    {
        [Test]
        public void ManyClassesShouldBePresentTest()
        {
            var codeQuery = new SolutionQuery(@"..\..\..\Roslinq.sln");
            var classes = codeQuery.Classes.Execute();
            Assert.IsNotNull(classes);
            Assert.IsTrue(classes.Count > 2);
        }
    }
}