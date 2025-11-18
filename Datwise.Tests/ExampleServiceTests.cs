using NUnit.Framework;
using Datwise.Services;

namespace Datwise.Tests
{
    [TestFixture]
    public class ExampleServiceTests
    {
        [Test]
        public void GetExample_Returns_Sample()
        {
            var svc = new ExampleService();
            var result = svc.GetExample(1);
            Assert.IsNotNull(result);
            Assert.AreEqual("Sample", result.Name);
        }
    }
}
