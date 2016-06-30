using System;
using AlgorithmsCore;
using AlgorithmsCore.Options;
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
            var options = new BaseOptions(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
            var graph = new BasicGraph(null, null);
            var rnd = new Random(Environment.TickCount);
            var aspg = new Aspg(options, graph, rnd);

            //var quality = aspg.GetQuality();
        }
    }
}
