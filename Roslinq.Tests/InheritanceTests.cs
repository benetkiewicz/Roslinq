namespace Roslinq.Tests
{
    using System.Linq;
    using System.Web.Mvc;
    using Core;
    using NUnit.Framework;

    [TestFixture]
    public class InheritanceTests
    {
        [Test]
        public void DirectClassInheritanceShouldBeRecognized()
        {
            var codeQuery = new ProjectQuery(@"..\..\..\RoslinqTestTarget\RoslinqTestTarget.csproj");
            var controllers = codeQuery.ClassesInheritingFrom(typeof(Controller));
            Assert.IsNotNull(controllers);
            Assert.IsTrue(controllers.Any());
            Assert.IsNotNull(controllers.FirstOrDefault(x => x == "HomeController"));
        }

        [Test]
        public void IndirectClassInheritanceShouldBeRecognized()
        {
            var codeQuery = new ProjectQuery(@"..\..\..\RoslinqTestTarget\RoslinqTestTarget.csproj");
            var controllers = codeQuery.ClassesInheritingFrom(typeof(Controller));
            Assert.IsNotNull(controllers);
            Assert.IsTrue(controllers.Any());
            Assert.IsNotNull(controllers.FirstOrDefault(x => x == "AdminReportingController"));
        }

        [Test]
        public void QueryClassesShouldReturnAllClasses()
        {
            var codeQuery = new ProjectQuery(@"..\..\..\RoslinqTestTarget\RoslinqTestTarget.csproj");
            var classes = codeQuery.Classes;
            Assert.IsNotNull(classes);
            Assert.IsTrue(classes.Any());
        }
    }
}
