using System;
using System.Linq;
using Basic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicTests
{

    [TestClass]
    public class AntSystemTests
    {
        private Random _rnd = new Random(Environment.TickCount);

        [TestMethod]
        public void Treil_AllTrailsInitilized_Success()
        {
            var numberOfRegions = 3;
            var maxNumberOfVertices = 7;
            var antSystem = new AntSystem(_rnd, numberOfRegions, maxNumberOfVertices);

            for (var i = 0; i < numberOfRegions; i++)
            {
                Assert.IsNotNull(antSystem.Treil[i]);
            }
        }

        [TestMethod]
        public void SumOfWeight_AllZerosSet_Success()
        {
            var numberOfRegions = 3;
            var maxNumberOfVertices = 7;
            var antSystem = new AntSystem(_rnd, numberOfRegions, maxNumberOfVertices);

            var sumOfWeights = antSystem.SumOfRegionWeight.Sum();

            Assert.AreEqual(0M, sumOfWeights);
        }

        [TestMethod]
        public void InitializeTreils_AllRegionsInitialized_Success()
        {
            var numberOfRegions = 3;
            var maxNumberOfVertices = 7;
            var antSystem = new AntSystem(_rnd, numberOfRegions, maxNumberOfVertices);

            antSystem.InitializeTreils();

            Assert.AreEqual(3, antSystem.Treil.Count);
        }

        [TestMethod]
        public void InitializeTreils_AllRegionsInitializedInRange_Success()
        {
            var numberOfRegions = 3;
            var maxNumberOfVertices = 7;
            var antSystem = new AntSystem(_rnd, numberOfRegions, maxNumberOfVertices);

            antSystem.InitializeTreils();

            for (var i = 0; i < numberOfRegions; i++)
            {
                int velueOfFirstVerticeInRegion = antSystem.Treil[i].Single();
                Assert.IsTrue(velueOfFirstVerticeInRegion < 8);
                Assert.IsTrue(velueOfFirstVerticeInRegion > 0);
            }
        }

        [TestMethod]
        public void InitializeTreils_FreeVertices_Success()
        {
            var numberOfRegions = 3;
            var maxNumberOfVertices = 7;
            var antSystem = new AntSystem(_rnd, numberOfRegions, maxNumberOfVertices);

            antSystem.InitializeTreils();

            Assert.AreEqual(maxNumberOfVertices-numberOfRegions, antSystem.freeVertices.Count);
        }

        [TestMethod]
        public void InitializeTreils_PassersVertices_Success()
        {
            var numberOfRegions = 3;
            var maxNumberOfVertices = 7;
            var antSystem = new AntSystem(_rnd, numberOfRegions, maxNumberOfVertices);

            antSystem.InitializeTreils();

            Assert.AreEqual(numberOfRegions, antSystem.passedVertices.Count);
        }
    }
}
