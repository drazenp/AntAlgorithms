using Basic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicTests
{
    [TestClass]
    public class AspgTests
    {
        [TestMethod]
        public void GetQuality_Success()
        {
            var options = new Options(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1M);
            var graph = new Graph(null, null);
            var aspg = new Aspg(options, graph);

            var quality = aspg.GetQuality();
        }

        [TestMethod]
        public void MaxAllowedWeight_Success()
        {
            var options = new Options(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1M);

            var aspg = new Aspg(options, null);

            var maxAllowedWeight = aspg.GetMaxAllowedWeight(new[] { 1, 2, 3, 4, 5, 1 });

            Assert.AreEqual(5.5M, maxAllowedWeight);
        }
    }
}
