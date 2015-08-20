using System.Collections.Generic;
using Basic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BasicTests
{
    [TestClass]
    public class AspgTests
    {
        [TestMethod]
        public void GetQuality_Success()
        {
            var options = new Options(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
            var graph = new Graph(null, null);
            var aspg = new Aspg(options, graph);

            //var quality = aspg.GetQuality();
        }

        [TestMethod]
        public void MaxAllowedWeight_Success()
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

            var aspg = new Aspg(options, mockGraph.Object);

            var maxAllowedWeight = aspg.GetMaxAllowedWeight();

            Assert.AreEqual(16.5D, maxAllowedWeight);
        }
    }
}
