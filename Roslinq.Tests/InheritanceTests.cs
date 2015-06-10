namespace Roslinq.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class InheritanceTests
    {
        [Test]
        public void DirectInheritanceTest()
        {
            Assert.IsTrue(InheritanceHelper.InheritsFrom(typeof(ArgumentException), typeof(SystemException)));
        }

        [Test]
        public void IndirectInheritanceTest()
        {
            Assert.IsTrue(InheritanceHelper.InheritsFrom(typeof(ArgumentException), typeof(object)));
        }

        [Test]
        public void ObjectInheritanceTest()
        {
            Assert.IsFalse(InheritanceHelper.InheritsFrom(typeof(object), typeof(object)));
        }
    }
}
