namespace Roslinq.Tests
{
    using System.Linq;
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
            Assert.IsNotNull(methods.FirstOrDefault(x => x.Name == "About"));
        }
    }
}
