using System;
using System.Collections.Generic;
using System.Linq;
using Basic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
            var verticesWeights = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);

            var antSystem = new AntSystem(_rnd, numberOfRegions, mockGraph.Object);

            for (var i = 0; i < numberOfRegions; i++)
            {
                Assert.IsNotNull(antSystem.Treil[i]);
            }
        }

        [TestMethod]
        public void SumOfWeight_AllZerosSet_Success()
        {
            const int numberOfRegions = 3;
            var verticesWeights = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);

            var antSystem = new AntSystem(_rnd, numberOfRegions, mockGraph.Object);

            var sumOfWeights = antSystem.SumOfRegionWeight.Sum();

            Assert.AreEqual(0M, sumOfWeights);
        }

        [TestMethod]
        public void FreeVertices_Initialized_Success()
        {
            const int numberOfRegions = 3;
            var verticesWeights = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);

            var antSystem = new AntSystem(_rnd, numberOfRegions, mockGraph.Object);

            Assert.AreEqual(verticesWeights.Count, antSystem.FreeVertices.Count);
        }

        [TestMethod]
        public void WeightsOfColonies_Initialized_Success()
        {
            const int numberOfRegions = 3;
            var verticesWeights = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);

            var antSystem = new AntSystem(_rnd, numberOfRegions, mockGraph.Object);

            Assert.IsNotNull(antSystem.WeightOfColonies);
            for (int i = 0; i < numberOfRegions; i++)
            {
                Assert.AreEqual(0, antSystem.WeightOfColonies[i]);
            }
        }

        [TestMethod]
        public void PassedVertices_Initialized_Success()
        {
            const int numberOfRegions = 3;
            var verticesWeights = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);

            var antSystem = new AntSystem(_rnd, numberOfRegions, mockGraph.Object);

            Assert.IsNotNull(antSystem.PassedVertices);
            Assert.AreEqual(0, antSystem.PassedVertices.Count);
        }

        [TestMethod]
        public void SumaKriterijumaOptimalnosti_Initialized_Success()
        {
            const int numberOfRegions = 3;
            var verticesWeights = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);

            var antSystem = new AntSystem(_rnd, numberOfRegions, mockGraph.Object);

            Assert.IsNotNull(antSystem.EdgesWeightOfColonies);
            for (int i = 0; i < numberOfRegions; i++)
            {
                Assert.AreEqual(0, antSystem.EdgesWeightOfColonies[i]);
            }
        }

        [TestMethod]
        public void InitializeTreils_AllRegionsInitialized_Success()
        {
            const int numberOfRegions = 3;
            var verticesWeights = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            var edgesWeights = new int[verticesWeights.Count, verticesWeights.Count];
            edgesWeights[0, 1] = 5;
            mockGraph.SetupGet(prop => prop.EdgesWeights).Returns(edgesWeights);

            var antSystem = new AntSystem(_rnd, numberOfRegions, mockGraph.Object);

            antSystem.InitializeTreils();

            Assert.AreEqual(3, antSystem.Treil.Count);
        }

        [TestMethod]
        public void InitializeTreils_AllRegionsInitializedInRange_Success()
        {
            const int numberOfRegions = 3;
            var verticesWeights = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            var edgesWeights = new int[verticesWeights.Count, verticesWeights.Count];
            edgesWeights[0, 1] = 5;
            mockGraph.SetupGet(prop => prop.EdgesWeights).Returns(edgesWeights);

            var antSystem = new AntSystem(_rnd, numberOfRegions, mockGraph.Object);

            antSystem.InitializeTreils();

            for (var i = 0; i < numberOfRegions; i++)
            {
                int velueOfFirstVerticeInRegion = antSystem.Treil[i].Single();
                Assert.IsTrue(velueOfFirstVerticeInRegion <= 8);
                Assert.IsTrue(velueOfFirstVerticeInRegion >= 0);
            }
        }

        [TestMethod]
        public void InitializeTreils_FreeVertices_Success()
        {
            const int numberOfRegions = 3;
            var verticesWeights = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            var edgesWeights = new int[verticesWeights.Count, verticesWeights.Count];
            edgesWeights[0, 1] = 5;
            mockGraph.SetupGet(prop => prop.EdgesWeights).Returns(edgesWeights);

            var antSystem = new AntSystem(_rnd, numberOfRegions, mockGraph.Object);

            antSystem.InitializeTreils();

            Assert.AreEqual(verticesWeights.Count - numberOfRegions, antSystem.FreeVertices.Count);
        }

        [TestMethod]
        public void InitializeTreils_PassersVertices_Success()
        {
            const int numberOfRegions = 3;
            var verticesWeights = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            var edgesWeights = new int[verticesWeights.Count, verticesWeights.Count];
            edgesWeights[0, 1] = 5;
            mockGraph.SetupGet(prop => prop.EdgesWeights).Returns(edgesWeights);

            var antSystem = new AntSystem(_rnd, numberOfRegions, mockGraph.Object);

            antSystem.InitializeTreils();

            Assert.AreEqual(numberOfRegions, antSystem.PassedVertices.Count);
        }

        [TestMethod]
        public void InitializeTreils_ColoniesWeightsSet_Success()
        {
            const int numberOfRegions = 3;
            var verticesWeights = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            var edgesWeights = new int[verticesWeights.Count, verticesWeights.Count];
            edgesWeights[0, 1] = 5;
            mockGraph.SetupGet(prop => prop.EdgesWeights).Returns(edgesWeights);

            var antSystem = new AntSystem(_rnd, numberOfRegions, mockGraph.Object);

            antSystem.InitializeTreils();

            for (int i = 0; i < numberOfRegions; i++)
            {
                Assert.AreEqual(antSystem.Treil[i].First(), antSystem.WeightOfColonies[i]);
            }
        }

        [TestMethod]
        public void InitializeTreils_EdgesWeightOfColoniesSet_Success()
        {
            const int numberOfRegions = 3;
            var verticesWeights = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            var edgesWeights = new int[verticesWeights.Count, verticesWeights.Count];
            edgesWeights[0, 1] = 5;
            mockGraph.SetupGet(prop => prop.EdgesWeights).Returns(edgesWeights);

            var antSystem = new AntSystem(_rnd, numberOfRegions, mockGraph.Object);

            antSystem.InitializeTreils();

            for (int i = 0; i < numberOfRegions; i++)
            {
                Assert.AreEqual(0, antSystem.EdgesWeightOfColonies[i]);
            }
        }

        [TestMethod]
        public void AddFreeVertexToTreil_AddVertex_Success()
        {
            const int numberOfRegions = 3;
            var verticesWeights = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            var edgesWeights = new int[verticesWeights.Count, verticesWeights.Count];
            edgesWeights[0, 1] = 5;
            mockGraph.SetupGet(prop => prop.EdgesWeights).Returns(edgesWeights);

            var antSystem = new AntSystem(_rnd, numberOfRegions, mockGraph.Object);

            antSystem.AddFreeVertexToTreil(0, 1);

            Assert.AreEqual(1, antSystem.Treil[0].Count);
            Assert.AreEqual(0, antSystem.Treil[1].Count);
            Assert.AreEqual(0, antSystem.Treil[2].Count);
            Assert.AreEqual(1, antSystem.PassedVertices.Count);
            Assert.AreEqual(verticesWeights.Count - 1, antSystem.FreeVertices.Count);
        }

        [TestMethod]
        public void GetNextColony_AfterInitialization_Success()
        {
            const int numberOfRegions = 3;
            var verticesWeights = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            var edgesWeights = new int[verticesWeights.Count, verticesWeights.Count];
            edgesWeights[0, 1] = 5;
            mockGraph.SetupGet(prop => prop.EdgesWeights).Returns(edgesWeights);

            var antSystem = new AntSystem(_rnd, numberOfRegions, mockGraph.Object);

            antSystem.InitializeTreils();

            var nextColonyIndex = antSystem.GetNextColony();
            var nextColonyWeightSum = antSystem.WeightOfColonies[nextColonyIndex];

            for (int i = 0; i < numberOfRegions; i++)
            {
                Assert.IsTrue(antSystem.WeightOfColonies[i] >= nextColonyWeightSum);
            }
        }
    }
}
