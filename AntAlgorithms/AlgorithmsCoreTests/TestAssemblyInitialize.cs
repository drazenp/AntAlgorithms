using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgorithmsCoreTests
{
    [TestClass]
    public static class TestAssemblyInitialize
    {
        [AssemblyInitialize]
        public static void Configure(TestContext tc)
        {
            XmlConfigurator.Configure();
        }
    }
}
