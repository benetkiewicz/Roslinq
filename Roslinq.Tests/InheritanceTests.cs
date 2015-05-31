﻿namespace Roslinq.Tests
{
    using System;
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
            var controllers = codeQuery.Classes.InheritingFrom(typeof(Controller)).Execute();
            Assert.IsNotNull(controllers);
            Assert.IsTrue(controllers.Any());
            Assert.IsNotNull(controllers.FirstOrDefault(x => x.ClassName == "RoslinqTestTarget.Controllers.HomeController"));
        }

        [Test]
        public void IndirectClassInheritanceShouldBeRecognized()
        {
            var codeQuery = new ProjectQuery(@"..\..\..\RoslinqTestTarget\RoslinqTestTarget.csproj");
            var controllers = codeQuery.Classes.InheritingFrom(typeof(Controller)).Execute();
            Assert.IsNotNull(controllers);
            Assert.IsTrue(controllers.Any());
            Assert.IsNotNull(controllers.FirstOrDefault(x => x.ClassName == "RoslinqTestTarget.Controllers.AdminReportingController"));
        }

        [Test]
        public void QueryClassesShouldReturnAllClasses()
        {
            var codeQuery = new ProjectQuery(@"..\..\..\RoslinqTestTarget\RoslinqTestTarget.csproj");
            var classes = codeQuery.Classes.Execute();
            Assert.IsNotNull(classes);
            Assert.IsTrue(classes.Any());
        }

        [Test]
        public void QueryClassesShouldReturnClassImplementingInterface()
        {
            var codeQuery = new ProjectQuery(@"..\..\..\RoslinqTestTarget\RoslinqTestTarget.csproj");
            var classes = codeQuery.Classes.ImplementingInterface("IController").Execute();
            Assert.IsNotNull(classes);
            Assert.IsTrue(classes.Any());
            Assert.IsNotNull(classes.FirstOrDefault(x => x.ClassName == "RoslinqTestTarget.Controllers.AdminController"));
        }

        [Test]
        public void QueryClassesShouldReturnClassWithAttributeApplied()
        {
            var codeQuery = new ProjectQuery(@"..\..\..\RoslinqTestTarget\RoslinqTestTarget.csproj");
            var classes = codeQuery.Classes.WithAttribute(typeof(SerializableAttribute)).Execute();
            Assert.IsNotNull(classes);
            Assert.IsTrue(classes.Any());
            Assert.IsNotNull(classes.FirstOrDefault(x => x.ClassName == "RoslinqTestTarget.Models.SerializableModel"));
        }
    }
}
