namespace Roslinq.Tests.Integration
{
    using Core;
    using NUnit.Framework;

    [TestFixture]
    public class ProjectTests
    {
        [Test]
        public void ManyClassesShouldBePresentTest()
        {
            var codeQuery = new ProjectQuery(@"..\..\..\RoslinqTestTarget\RoslinqTestTarget.csproj");
            var classes = codeQuery.Classes.Execute();
            Assert.IsNotNull(classes);
            Assert.IsTrue(classes.Count > 2);
        } 
    }
}