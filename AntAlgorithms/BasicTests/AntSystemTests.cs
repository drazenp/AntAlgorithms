using System;
using System.Linq;
using Basic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicTests
{
    [TestClass]
    public class AntSystemTests
    {
        private readonly Random _rnd = new Random(Environment.TickCount);

        [TestMethod]
        public void Treil_AllTrailsInitilized_Success()
        {
            const int numberOfRegions = 3;
            const int numberOfVertices = 9;
            var antSystem = new AntSystem(_rnd, numberOfRegions, numberOfVertices);

            for (var i = 0; i < numberOfRegions; i++)
            {
                Assert.IsNotNull(antSystem.Treil[i]);
            }
        }

        [TestMethod]
        public void SumOfWeight_AllZerosSet_Success()
        {
            const int numberOfRegions = 3;
            const int numberOfVertices = 9;
            var antSystem = new AntSystem(_rnd, numberOfRegions, numberOfVertices);

            var sumOfWeights = antSystem.SumOfRegionWeight.Sum();

            Assert.AreEqual(0M, sumOfWeights);
        }

        [TestMethod]
        public void FreeVertices_Initialized_Success()
        {
            const int numberOfRegions = 3;
            const int numberOfVertices = 9;
            var antSystem = new AntSystem(_rnd, numberOfRegions, numberOfVertices);

            Assert.AreEqual(numberOfVertices, antSystem.FreeVertices.Count);
        }

        [TestMethod]
        public void PassedVertices_Initialized_Success()
        {
            const int numberOfRegions = 3;
            const int numberOfVertices = 9;
            var antSystem = new AntSystem(_rnd, numberOfRegions, numberOfVertices);

            Assert.IsNotNull(antSystem.PassedVertices);
            Assert.AreEqual(0, antSystem.PassedVertices.Count);
        }

        [TestMethod]
        public void InitializeTreils_AllRegionsInitialized_Success()
        {
            const int numberOfRegions = 3;
            const int numberOfVertices = 9;
            var antSystem = new AntSystem(_rnd, numberOfRegions, numberOfVertices);

            antSystem.InitializeTreils();

            Assert.AreEqual(3, antSystem.Treil.Count);
        }

        [TestMethod]
        public void InitializeTreils_AllRegionsInitializedInRange_Success()
        {
            const int numberOfRegions = 3;
            const int numberOfVertices = 9;
            var antSystem = new AntSystem(_rnd, numberOfRegions, numberOfVertices);

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
            const int numberOfRegions = 3;
            const int numberOfVertices = 9;
            var antSystem = new AntSystem(_rnd, numberOfRegions, numberOfVertices);

            antSystem.InitializeTreils();

            Assert.AreEqual(numberOfVertices - numberOfRegions, antSystem.FreeVertices.Count);
        }

        [TestMethod]
        public void InitializeTreils_PassersVertices_Success()
        {
            const int numberOfRegions = 3;
            const int numberOfVertices = 9;
            var antSystem = new AntSystem(_rnd, numberOfRegions, numberOfVertices);

            antSystem.InitializeTreils();

            Assert.AreEqual(numberOfRegions, antSystem.PassedVertices.Count);
        }
    }
}
