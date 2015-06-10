namespace Roslinq.Tests.Integration
{
    using System.Linq;
    using System.Web.Mvc;
    using Core;
    using NUnit.Framework;
    using RoslinqTestTarget.Models;

    [TestFixture]
    public class MethodTests
    {
        private ProjectQuery codeQuery;
        [TestFixtureSetUp]
        public void SetUp()
        {
            this.codeQuery = new ProjectQuery(@"..\..\..\RoslinqTestTarget\RoslinqTestTarget.csproj");
        }

        [Test]
        public void ContactMethodFromHomeControllerShouldBeRecognized()
        {
            var methods = codeQuery.Classes.Methods.Execute();
            Assert.IsNotNull(methods);
            Assert.IsTrue(methods.Any());
            Assert.IsNotNull(methods.FirstOrDefault(x => x.MethodName == "About"));
        }

        [Test]
        public void MethodReturningPartialFromHomeControllerShouldBeRecognized()
        {
            var methods = codeQuery.Classes.Methods.ReturningType(typeof(PartialViewResult)).Execute();
            Assert.IsNotNull(methods);
            Assert.IsTrue(methods.Any());
            Assert.IsNotNull(methods.FirstOrDefault(x => x.MethodName == "Partial"));
        }

        [Test]
        public void MethodWithModelInputParamShouldBeRecognized()
        {
            var methods = codeQuery.Classes.Methods.WithParameterType(typeof(SerializableModel)).Execute();
            Assert.IsNotNull(methods);
            Assert.IsTrue(methods.Any());
            Assert.IsNotNull(methods.FirstOrDefault(x => x.MethodName == "Baz"));
        }

        [Test]
        public void PrivateStaticMethodShouldBeFound()
        {
            var methods = codeQuery.Classes.Methods
                .WithModifier(MethodModifier.Static)
                .WithModifier(MethodModifier.Private)
                .Execute();
            Assert.IsNotNull(methods);
            Assert.AreEqual(1, methods.Count());
            Assert.IsNotNull(methods.FirstOrDefault(x => x.MethodName == "MethodB"));
        }
    }
}
