namespace Roslinq.Tests.Integration
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Core;
    using NUnit.Framework;

    [TestFixture]
    public class ClassTests
    {
        private ProjectQuery codeQuery;
        [TestFixtureSetUp]
        public void SetUp()
        {
            this.codeQuery = new ProjectQuery(@"..\..\..\RoslinqTestTarget\RoslinqTestTarget.csproj");
        }

        [Test]
        public void DirectClassInheritanceShouldBeRecognized()
        {
            var controllers = codeQuery.Classes.InheritingFrom(typeof(Controller)).Execute();
            Assert.IsNotNull(controllers);
            Assert.IsTrue(controllers.Any());
            Assert.IsNotNull(controllers.FirstOrDefault(x => x.ClassName == "RoslinqTestTarget.Controllers.HomeController"));
        }

        [Test]
        public void IndirectClassInheritanceShouldBeRecognized()
        {
            var controllers = codeQuery.Classes.InheritingFrom(typeof(Controller)).Execute();
            Assert.IsNotNull(controllers);
            Assert.IsTrue(controllers.Any());
            Assert.IsNotNull(controllers.FirstOrDefault(x => x.ClassName == "RoslinqTestTarget.Controllers.AdminReportingController"));
        }

        [Test]
        public void QueryClassesShouldReturnAllClasses()
        {
            var classes = codeQuery.Classes.Execute();
            Assert.IsNotNull(classes);
            Assert.IsTrue(classes.Any());
        }

        [Test]
        public void QueryClassesShouldReturnClassImplementingInterface()
        {
            var classes = codeQuery.Classes.ImplementingInterface(typeof(IController)).Execute();
            Assert.IsNotNull(classes);
            Assert.IsTrue(classes.Any());
            Assert.IsNotNull(classes.FirstOrDefault(x => x.ClassName == "RoslinqTestTarget.Controllers.AdminController"));
        }

        [Test]
        public void QueryClassesShouldReturnClassWithAttributeApplied()
        {
            var classes = codeQuery.Classes.WithAttribute(typeof(SerializableAttribute)).Execute();
            Assert.IsNotNull(classes);
            Assert.IsTrue(classes.Any());
            Assert.IsNotNull(classes.FirstOrDefault(x => x.ClassName == "RoslinqTestTarget.Models.SerializableModel"));
        }

        [Test]
        public void QueryClassesShouldReturnClassWithInternalModifier()
        {
            var classes = codeQuery.Classes.WithModifier(ClassModifier.Internal).Execute();
            Assert.IsNotNull(classes);
            Assert.IsTrue(classes.Any());
            Assert.IsNotNull(classes.FirstOrDefault(x => x.ClassName == "RoslinqTestTarget.AppCode.InternalLogic"));
        }
    }
}
