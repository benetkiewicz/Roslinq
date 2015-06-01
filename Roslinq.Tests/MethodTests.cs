namespace Roslinq.Tests
{
    using System.Linq;
    using System.Web.Mvc;
    using Core;
    using NUnit.Framework;
    using RoslinqTestTarget.Models;

    public class MethodTests
    {
        [Test]
        public void ContactMethodFromHomeControllerShouldBeRecognized()
        {
            var codeQuery = new ProjectQuery(@"..\..\..\RoslinqTestTarget\RoslinqTestTarget.csproj");
            var methods = codeQuery.Classes.Methods.Execute();
            Assert.IsNotNull(methods);
            Assert.IsTrue(methods.Any());
            Assert.IsNotNull(methods.FirstOrDefault(x => x.MethodName == "About"));
        }

        [Test]
        public void MethodReturningPartialFromHomeControllerShouldBeRecognized()
        {
            var codeQuery = new ProjectQuery(@"..\..\..\RoslinqTestTarget\RoslinqTestTarget.csproj");
            var methods = codeQuery.Classes.Methods.ReturningType(typeof(PartialViewResult)).Execute();
            Assert.IsNotNull(methods);
            Assert.IsTrue(methods.Any());
            Assert.IsNotNull(methods.FirstOrDefault(x => x.MethodName == "Partial"));
        }

        [Test]
        public void MethodWithModelInputParamShouldBeRecognized()
        {
            var codeQuery = new ProjectQuery(@"..\..\..\RoslinqTestTarget\RoslinqTestTarget.csproj");
            var methods = codeQuery.Classes.Methods.WithParameterType(typeof(SerializableModel)).Execute();
            Assert.IsNotNull(methods);
            Assert.IsTrue(methods.Any());
            Assert.IsNotNull(methods.FirstOrDefault(x => x.MethodName == "Baz"));
        }

        [Test]
        public void PrivateStaticMethodShouldBeFound()
        {
            var codeQuery = new ProjectQuery(@"..\..\..\RoslinqTestTarget\RoslinqTestTarget.csproj");
            var methods = codeQuery.Classes.Methods
                .WithModifier(Modifiers.Method.Static)
                .WithModifier(Modifiers.Method.Private)
                .Execute();
            Assert.IsNotNull(methods);
            Assert.AreEqual(1, methods.Count());
            Assert.IsNotNull(methods.FirstOrDefault(x => x.MethodName == "MethodB"));
        }
    }
}
