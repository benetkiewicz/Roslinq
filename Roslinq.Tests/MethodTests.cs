namespace Roslinq.Tests
{
    using System.Linq;
    using System.Web.Mvc;
    using Core;
    using NUnit.Framework;

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
    }
}
