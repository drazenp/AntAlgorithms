using System;
using System.Collections.Generic;
using System.Linq;
using AlgorithmsCore;
using AlgorithmsCore.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AlgorithmsCoreTests
{
    [TestClass]
    public class AntSystemBaseTests
    {
        private readonly Random _rnd = new Random(Environment.TickCount);
        /*
        [TestMethod]
        public void InitializeTreils_AllRegionsInitialized_Success()
        {
            var options = new Options(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
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
            var edgesWeights = new int[verticesWeights.Count, verticesWeights.Count];
            edgesWeights[0, 1] = 5;
            mockGraph.SetupGet(prop => prop.EdgesWeights).Returns(edgesWeights);

            var antSystem = new AntSystemBase(_rnd, options, mockGraph.Object);

            antSystem.InitializeTreils();

            Assert.AreEqual(3, antSystem.Treil.Count);
        }
        */
        //[TestMethod]
        //public void InitializeTreils_AllRegionsInitializedInRange_Success()
        //{
        //    var options = new Options(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
        //    var verticesWeights = new List<Vertex>
        //    {
        //        new Vertex(0, 4),
        //        new Vertex(1, 3),
        //        new Vertex(2, 8),
        //        new Vertex(3, 7),
        //        new Vertex(4, 5),
        //        new Vertex(5, 3),
        //        new Vertex(6, 6),
        //        new Vertex(7, 7),
        //        new Vertex(8, 2)
        //    };
        //    var mockGraph = new Mock<IGraph>();
        //    mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
        //    mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
        //    var edgesWeights = new int[verticesWeights.Count, verticesWeights.Count];
        //    edgesWeights[0, 1] = 5;
        //    mockGraph.SetupGet(prop => prop.EdgesWeights).Returns(edgesWeights);

        //    var antSystem = new AntSystemBase(_rnd, options, mockGraph.Object);

        //    antSystem.InitializeTreils();

        //    for (var i = 0; i < options.NumberOfRegions; i++)
        //    {
        //        int velueOfFirstVerticeInRegion = antSystem.Treil[i].Single().Weight;
        //        Assert.IsTrue(velueOfFirstVerticeInRegion <= 8);
        //        Assert.IsTrue(velueOfFirstVerticeInRegion >= 0);
        //    }
        //}

        //[TestMethod]
        //public void InitializeTreils_FreeVertices_Success()
        //{
        //    var options = new Options(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
        //    var verticesWeights = new List<Vertex>
        //    {
        //        new Vertex(0, 4),
        //        new Vertex(1, 3),
        //        new Vertex(2, 8),
        //        new Vertex(3, 7),
        //        new Vertex(4, 5),
        //        new Vertex(5, 3),
        //        new Vertex(6, 6),
        //        new Vertex(7, 7),
        //        new Vertex(8, 2)
        //    };
        //    var mockGraph = new Mock<IGraph>();
        //    mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
        //    mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
        //    var edgesWeights = new int[verticesWeights.Count, verticesWeights.Count];
        //    edgesWeights[0, 1] = 5;
        //    mockGraph.SetupGet(prop => prop.EdgesWeights).Returns(edgesWeights);

        //    var antSystem = new AntSystemBase(_rnd, options, mockGraph.Object);

        //    antSystem.InitializeTreils();

        //    Assert.AreEqual(verticesWeights.Count - options.NumberOfRegions, antSystem.FreeVertices.Count);
        //}

        //[TestMethod]
        //public void InitializeTreils_PassersVertices_Success()
        //{
        //    var options = new Options(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
        //    var verticesWeights = new List<Vertex>
        //    {
        //        new Vertex(0, 4),
        //        new Vertex(1, 3),
        //        new Vertex(2, 8),
        //        new Vertex(3, 7),
        //        new Vertex(4, 5),
        //        new Vertex(5, 3),
        //        new Vertex(6, 6),
        //        new Vertex(7, 7),
        //        new Vertex(8, 2)
        //    };
        //    var mockGraph = new Mock<IGraph>();
        //    mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
        //    mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
        //    var edgesWeights = new int[verticesWeights.Count, verticesWeights.Count];
        //    edgesWeights[0, 1] = 5;
        //    mockGraph.SetupGet(prop => prop.EdgesWeights).Returns(edgesWeights);

        //    var antSystem = new AntSystemBase(_rnd, options, mockGraph.Object);

        //    antSystem.InitializeTreils();

        //    Assert.AreEqual(options.NumberOfRegions, antSystem.PassedVertices.Count);
        //}

        //[TestMethod]
        //public void InitializeTreils_ColoniesWeightsSet_Success()
        //{
        //    var options = new Options(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
        //    var verticesWeights = new List<Vertex>
        //    {
        //        new Vertex(0, 4),
        //        new Vertex(1, 3),
        //        new Vertex(2, 8),
        //        new Vertex(3, 7),
        //        new Vertex(4, 5),
        //        new Vertex(5, 3),
        //        new Vertex(6, 6),
        //        new Vertex(7, 7),
        //        new Vertex(8, 2)
        //    };
        //    var mockGraph = new Mock<IGraph>();
        //    mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
        //    mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
        //    var edgesWeights = new int[verticesWeights.Count, verticesWeights.Count];
        //    edgesWeights[0, 1] = 5;
        //    mockGraph.SetupGet(prop => prop.EdgesWeights).Returns(edgesWeights);

        //    var antSystem = new AntSystemBase(_rnd, options, mockGraph.Object);

        //    antSystem.InitializeTreils();

        //    for (int i = 0; i < options.NumberOfRegions; i++)
        //    {
        //        Assert.AreEqual(antSystem.Treil[i].First().Weight, antSystem.WeightOfColonies[i]);
        //    }
        //}

        //[TestMethod]
        //public void InitializeTreils_EdgesWeightOfColoniesSet_Success()
        //{
        //    var options = new Options(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
        //    var verticesWeights = new List<Vertex>
        //    {
        //        new Vertex(0, 4),
        //        new Vertex(1, 3),
        //        new Vertex(2, 8),
        //        new Vertex(3, 7),
        //        new Vertex(4, 5),
        //        new Vertex(5, 3),
        //        new Vertex(6, 6),
        //        new Vertex(7, 7),
        //        new Vertex(8, 2)
        //    };
        //    var mockGraph = new Mock<IGraph>();
        //    mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
        //    mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
        //    var edgesWeights = new int[verticesWeights.Count, verticesWeights.Count];
        //    edgesWeights[0, 1] = 5;
        //    mockGraph.SetupGet(prop => prop.EdgesWeights).Returns(edgesWeights);

        //    var antSystem = new AntSystemBase(_rnd, options, mockGraph.Object);

        //    antSystem.InitializeTreils();

        //    for (int i = 0; i < options.NumberOfRegions; i++)
        //    {
        //        Assert.AreEqual(0, antSystem.EdgesWeightOfColonies[i]);
        //    }
        //}

        //[TestMethod]
        //public void AddFreeVertexToTreil_AddVertex_Success()
        //{
        //    var options = new Options(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
        //    var verticesWeights = new List<Vertex>
        //    {
        //        new Vertex(0, 4),
        //        new Vertex(1, 3),
        //        new Vertex(2, 8),
        //        new Vertex(3, 7),
        //        new Vertex(4, 5),
        //        new Vertex(5, 3),
        //        new Vertex(6, 6),
        //        new Vertex(7, 7),
        //        new Vertex(8, 2)
        //    };
        //    var mockGraph = new Mock<IGraph>();
        //    mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
        //    mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
        //    var edgesWeights = new int[verticesWeights.Count, verticesWeights.Count];
        //    edgesWeights[0, 1] = 5;
        //    mockGraph.SetupGet(prop => prop.EdgesWeights).Returns(edgesWeights);

        //    var antSystem = new AntSystemBase(_rnd, options, mockGraph.Object);

        //    antSystem.AddFreeVertexToTreil(0, new Vertex(1, 1));

        //    Assert.AreEqual(1, antSystem.Treil[0].Count);
        //    Assert.AreEqual(0, antSystem.Treil[1].Count);
        //    Assert.AreEqual(0, antSystem.Treil[2].Count);
        //    Assert.AreEqual(1, antSystem.PassedVertices.Count);
        //    Assert.AreEqual(verticesWeights.Count, antSystem.FreeVertices.Count);
        //}

        //[TestMethod]
        //public void GetNextColony_AfterInitialization_Success()
        //{
        //    var options = new Options(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
        //    var verticesWeights = new List<Vertex>
        //    {
        //        new Vertex(0, 4),
        //        new Vertex(1, 3),
        //        new Vertex(2, 8),
        //        new Vertex(3, 7),
        //        new Vertex(4, 5),
        //        new Vertex(5, 3),
        //        new Vertex(6, 6),
        //        new Vertex(7, 7),
        //        new Vertex(8, 2)
        //    };
        //    var mockGraph = new Mock<IGraph>();
        //    mockGraph.SetupGet(prop => prop.NumberOfVertices).Returns(verticesWeights.Count);
        //    mockGraph.SetupGet(prop => prop.VerticesWeights).Returns(verticesWeights);
        //    var edgesWeights = new int[verticesWeights.Count, verticesWeights.Count];
        //    edgesWeights[0, 1] = 5;
        //    mockGraph.SetupGet(prop => prop.EdgesWeights).Returns(edgesWeights);

        //    var antSystem = new AntSystemBase(_rnd, options, mockGraph.Object);

        //    antSystem.InitializeTreils();

        //    var nextColonyIndex = antSystem.GetNextColony();
        //    var nextColonyWeightSum = antSystem.WeightOfColonies[nextColonyIndex];

        //    for (int i = 0; i < options.NumberOfRegions; i++)
        //    {
        //        Assert.IsTrue(antSystem.WeightOfColonies[i] >= nextColonyWeightSum);
        //    }
        //}
    }
}
