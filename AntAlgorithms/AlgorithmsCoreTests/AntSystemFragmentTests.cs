﻿using System;
using System.Collections.Generic;
using System.Linq;
using AlgorithmsCore;
using AlgorithmsCore.Contracts;
using AlgorithmsCore.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AlgorithmsCoreTests
{
    [TestClass]
    public class AntSystemFragmentTests
    {
        [TestMethod]
        public void Treil_AllTrailsInitilized_Success()
        {
            var random = new Random(1);
            var options = new BaseOptions(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
            var verticesWeights = new List<Vertex>
                    {
                        new Vertex(0, 4),
                        new Vertex(1, 3),
                        new Vertex(2, 8),
                        new Vertex(3, 7),
                        new Vertex(4, 5),
                        new Vertex(5, 3),
                        new Vertex(6, 6),
                        new Vertex(7, 7),
                        new Vertex(8, 2)
                    };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            mockGraph.Setup(prop => prop.EdgesWeights).Returns(new int[verticesWeights.Count, verticesWeights.Count]);

            var antSystem = new WeightedAntSystemFragment(random, options, mockGraph.Object);

            for (var i = 0; i < options.NumberOfRegions; i++)
            {
                Assert.IsNotNull(antSystem.Treil[i]);
            }
        }

        [TestMethod]
        public void InitializeTreils_AllRegionsInitializedInRange_Success()
        {
            var random = new Random(1);
            var options = new BaseOptions(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
            var verticesWeights = new List<Vertex>
                    {
                        new Vertex(0, 4),
                        new Vertex(1, 3),
                        new Vertex(2, 8),
                        new Vertex(3, 7),
                        new Vertex(4, 5),
                        new Vertex(5, 3),
                        new Vertex(6, 6),
                        new Vertex(7, 7),
                        new Vertex(8, 2)
                    };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            mockGraph.Setup(prop => prop.EdgesWeights).Returns(new int[verticesWeights.Count, verticesWeights.Count]);

            var antSystem = new WeightedAntSystemFragment(random, options, mockGraph.Object);

            for (var i = 0; i < options.NumberOfRegions; i++)
            {
                int velueOfFirstVerticeInRegion = antSystem.Treil[i].Single().Weight;
                Assert.IsTrue(velueOfFirstVerticeInRegion <= 8);
                Assert.IsTrue(velueOfFirstVerticeInRegion >= 0);
            }
        }

        [TestMethod]
        public void SumOfWeight_AllZerosSet_Success()
        {
            var random = new Random(1);
            var options = new BaseOptions(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
            var verticesWeights = new List<Vertex>
                    {
                        new Vertex(0, 4),
                        new Vertex(1, 3),
                        new Vertex(2, 8),
                        new Vertex(3, 7),
                        new Vertex(4, 5),
                        new Vertex(5, 3),
                        new Vertex(6, 6),
                        new Vertex(7, 7),
                        new Vertex(8, 2)
                    };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            mockGraph.Setup(prop => prop.EdgesWeights).Returns(new int[verticesWeights.Count, verticesWeights.Count]);

            var antSystem = new WeightedAntSystemFragment(random, options, mockGraph.Object);

            var sumOfWeights = antSystem.WeightOfColonies.Sum();

            Assert.AreEqual(15, sumOfWeights);
        }

        [TestMethod]
        public void FreeVertices_Initialized_Success()
        {
            var random = new Random(1);
            var options = new BaseOptions(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
            var verticesWeights = new List<Vertex>
                    {
                        new Vertex(0, 4),
                        new Vertex(1, 3),
                        new Vertex(2, 8),
                        new Vertex(3, 7),
                        new Vertex(4, 5),
                        new Vertex(5, 3),
                        new Vertex(6, 6),
                        new Vertex(7, 7),
                        new Vertex(8, 2)
                    };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            mockGraph.Setup(prop => prop.EdgesWeights).Returns(new int[verticesWeights.Count, verticesWeights.Count]);

            var antSystem = new WeightedAntSystemFragment(random, options, mockGraph.Object);

            Assert.AreEqual(6, antSystem.FreeVertices.Count);
        }

        [TestMethod]
        public void WeightsOfColonies_Initialized_Success()
        {
            var random = new Random(1);
            var options = new BaseOptions(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
            var verticesWeights = new List<Vertex>
                    {
                        new Vertex(0, 4),
                        new Vertex(1, 3),
                        new Vertex(2, 8),
                        new Vertex(3, 7),
                        new Vertex(4, 5),
                        new Vertex(5, 3),
                        new Vertex(6, 6),
                        new Vertex(7, 7),
                        new Vertex(8, 2)
                    };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            mockGraph.Setup(prop => prop.EdgesWeights).Returns(new int[verticesWeights.Count, verticesWeights.Count]);

            var antSystem = new WeightedAntSystemFragment(random, options, mockGraph.Object);

            Assert.IsNotNull(antSystem.WeightOfColonies);

            Assert.AreEqual(8, antSystem.WeightOfColonies[0]);
            Assert.AreEqual(4, antSystem.WeightOfColonies[1]);
            Assert.AreEqual(3, antSystem.WeightOfColonies[2]);
            /*
            for (int i = 0; i < options.NumberOfRegions; i++)
            {
                Assert.AreEqual(antSystem.Treil[i].First().Weight, antSystem.WeightOfColonies[i]);
            }
            */
        }

        [TestMethod]
        public void PassedVertices_Initialized_Success()
        {
            var random = new Random(1);
            var options = new BaseOptions(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
            var verticesWeights = new List<Vertex>
                    {
                        new Vertex(0, 4),
                        new Vertex(1, 3),
                        new Vertex(2, 8),
                        new Vertex(3, 7),
                        new Vertex(4, 5),
                        new Vertex(5, 3),
                        new Vertex(6, 6),
                        new Vertex(7, 7),
                        new Vertex(8, 2)
                    };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            mockGraph.Setup(prop => prop.EdgesWeights).Returns(new int[verticesWeights.Count, verticesWeights.Count]);

            var antSystem = new WeightedAntSystemFragment(random, options, mockGraph.Object);

            Assert.IsNotNull(antSystem.PassedVertices);
            Assert.AreEqual(3, antSystem.PassedVertices.Count);
        }

        [TestMethod]
        public void AddFreeVertexToTreil_AddVertex_Success()
        {
            var random = new Random(1);
            var options = new BaseOptions(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
            var verticesWeights = new List<Vertex>
                    {
                        new Vertex(0, 4),
                        new Vertex(1, 3),
                        new Vertex(2, 8),
                        new Vertex(3, 7),
                        new Vertex(4, 5),
                        new Vertex(5, 3),
                        new Vertex(6, 6),
                        new Vertex(7, 7),
                        new Vertex(8, 2)
                    };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            mockGraph.Setup(prop => prop.EdgesWeights).Returns(new int[verticesWeights.Count, verticesWeights.Count]);

            var antSystem = new WeightedAntSystemFragment(random, options, mockGraph.Object);

            antSystem.AddFreeVertexToTreil(0, antSystem.FreeVertices.First());

            Assert.AreEqual(2, antSystem.Treil[0].Count);
            Assert.AreEqual(1, antSystem.Treil[1].Count);
            Assert.AreEqual(1, antSystem.Treil[2].Count);
            Assert.AreEqual(4, antSystem.PassedVertices.Count);
        }

        [TestMethod]
        public void SumaKriterijumaOptimalnosti_Initialized_Success()
        {
            var random = new Random(1);
            var options = new BaseOptions(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
            var verticesWeights = new List<Vertex>
                    {
                        new Vertex(0, 4),
                        new Vertex(1, 3),
                        new Vertex(2, 8),
                        new Vertex(3, 7),
                        new Vertex(4, 5),
                        new Vertex(5, 3),
                        new Vertex(6, 6),
                        new Vertex(7, 7),
                        new Vertex(8, 2)
                    };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            mockGraph.Setup(prop => prop.EdgesWeights).Returns(new int[verticesWeights.Count, verticesWeights.Count]);

            var antSystem = new WeightedAntSystemFragment(random, options, mockGraph.Object);

            Assert.IsNotNull(antSystem.EdgesWeightOfColonies);
            for (var i = 0; i < options.NumberOfRegions; i++)
            {
                Assert.AreEqual(0, antSystem.EdgesWeightOfColonies[i]);
            }
        }

        [TestMethod]
        public void GetSumOfOptimalityCriterion_Success()
        {
            var random = new Random(1);
            var options = new BaseOptions(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
            var verticesWeights = new List<Vertex>
                    {
                        new Vertex(0, 4),
                        new Vertex(1, 3),
                        new Vertex(2, 8),
                        new Vertex(3, 7),
                        new Vertex(4, 5),
                        new Vertex(5, 3),
                        new Vertex(6, 6),
                        new Vertex(7, 7),
                        new Vertex(8, 2)
                    };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            mockGraph.Setup(prop => prop.EdgesWeights).Returns(new int[verticesWeights.Count, verticesWeights.Count]);

            var antSystem = new WeightedAntSystemFragment(random, options, mockGraph.Object);

            var sumOfOptimalityCriterion = antSystem.SumOfOptimalityCriterion;

            Assert.AreEqual(6000, sumOfOptimalityCriterion);
        }

        [TestMethod]
        public void GetNextColoy_Success()
        {
            var random = new Random(1);
            var options = new BaseOptions(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
            var verticesWeights = new List<Vertex>
                    {
                        new Vertex(0, 4),
                        new Vertex(1, 3),
                        new Vertex(2, 8),
                        new Vertex(3, 7),
                        new Vertex(4, 5),
                        new Vertex(5, 3),
                        new Vertex(6, 6),
                        new Vertex(7, 7),
                        new Vertex(8, 2)
                    };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            mockGraph.Setup(prop => prop.EdgesWeights).Returns(new int[verticesWeights.Count, verticesWeights.Count]);

            var antSystem = new WeightedAntSystemFragment(random, options, mockGraph.Object);

            var nextColonyId = antSystem.GetNextColony();

            Assert.AreEqual(2, nextColonyId);
        }

        [TestMethod]
        public void GetNextColony_AfterInitialization_Success()
        {
            var random = new Random(1);
            var options = new BaseOptions(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
            var verticesWeights = new List<Vertex>
                    {
                        new Vertex(0, 4),
                        new Vertex(1, 3),
                        new Vertex(2, 8),
                        new Vertex(3, 7),
                        new Vertex(4, 5),
                        new Vertex(5, 3),
                        new Vertex(6, 6),
                        new Vertex(7, 7),
                        new Vertex(8, 2)
                    };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            mockGraph.Setup(prop => prop.EdgesWeights).Returns(new int[verticesWeights.Count, verticesWeights.Count]);

            var antSystem = new WeightedAntSystemFragment(random, options, mockGraph.Object);

            var nextColonyIndex = antSystem.GetNextColony();
            var nextColonyWeightSum = antSystem.WeightOfColonies[nextColonyIndex];

            for (int i = 0; i < options.NumberOfRegions; i++)
            {
                Assert.IsTrue(antSystem.WeightOfColonies[i] >= nextColonyWeightSum);
            }
        }

        [TestMethod]
        public void GetNextColony_AddVertex_Success()
        {
            var random = new Random(1);
            var options = new BaseOptions(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
            var verticesWeights = new List<Vertex>
                    {
                        new Vertex(0, 4),
                        new Vertex(1, 3),
                        new Vertex(2, 8),
                        new Vertex(3, 7),
                        new Vertex(4, 5),
                        new Vertex(5, 3),
                        new Vertex(6, 6),
                        new Vertex(7, 7),
                        new Vertex(8, 2)
                    };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            mockGraph.Setup(prop => prop.EdgesWeights).Returns(new int[verticesWeights.Count, verticesWeights.Count]);

            var antSystem = new WeightedAntSystemFragment(random, options, mockGraph.Object);

            antSystem.AddFreeVertexToTreil(0, antSystem.FreeVertices.First());

            var nextColonyIndex = antSystem.GetNextColony();
            var nextColonyWeightSum = antSystem.WeightOfColonies[nextColonyIndex];

            for (int i = 0; i < options.NumberOfRegions; i++)
            {
                Assert.IsTrue(antSystem.WeightOfColonies[i] >= nextColonyWeightSum);
            }
        }

        [TestMethod]
        public void CalculateProbability_Success()
        {
            var random = new Random(1);
            var options = new BaseOptions(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
            var verticesWeights = new List<Vertex>
                    {
                        new Vertex(0, 4),
                        new Vertex(1, 3),
                        new Vertex(2, 8),
                        new Vertex(3, 7),
                        new Vertex(4, 5),
                        new Vertex(5, 3),
                        new Vertex(6, 6),
                        new Vertex(7, 7),
                        new Vertex(8, 2)
                    };
            var mockGraph = new Mock<IGraph>();
            mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
            mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
            mockGraph.Setup(prop => prop.EdgesWeights).Returns(new int[verticesWeights.Count, verticesWeights.Count]);
            mockGraph.Setup(prop => prop.PheromoneMatrix).Returns(new double[verticesWeights.Count, verticesWeights.Count]);

            var antSystem = new WeightedAntSystemFragment(random, options, mockGraph.Object);

            var probability = antSystem.CalculateProbability(1);

            Assert.AreEqual(probability[0], 0);
        }
    }
}
