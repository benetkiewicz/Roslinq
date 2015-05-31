namespace Roslinq.Tests
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Core;
    using NUnit.Framework;
    using RoslinqTestTarget.Controllers;

    [TestFixture]
    public class InheritanceTests
    {
        [Test]
        public void DirectInheritanceTest()
        {
            Assert.IsTrue(InheritanceHelper.InheritsFrom(typeof(int), typeof(object)));
        }

        [Test]
        public void IndirectInheritanceTest()
        {
            Assert.IsTrue(InheritanceHelper.InheritsFrom(typeof(AdminReportingController), typeof(Controller)));
        }

        [Test]
        public void ObjectInheritanceTest()
        {
            Assert.IsFalse(InheritanceHelper.InheritsFrom(typeof(object), typeof(object)));
        }
    }
}
