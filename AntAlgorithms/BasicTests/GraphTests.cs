using System.Collections.Generic;
using System.IO;
using Basic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicTests
{
    [TestClass]
    public class GraphTests
    {
        private const string BasicVericesWeights = @"tc
                                                    4
                                                    3
                                                    8
                                                    7
                                                    5
                                                    3
                                                    6
                                                    7
                                                    2";

        private const string BasicEdgesWeights = @"t0 t1 tg
                                                    0 1 3
                                                    0 5 1
                                                    1 2 2
                                                    1 4 1
                                                    1 5 2
                                                    2 3 3
                                                    2 4 2
                                                    3 4 1
                                                    3 6 2
                                                    3 8 1
                                                    4 5 2
                                                    4 6 1
                                                    5 6 1
                                                    5 7 2
                                                    6 7 1
                                                    6 8 2
                                                    7 8 3";

        [TestMethod]
        public void ReadVerticesWeights_CorrectFormat_Success()
        {
            var graph = new Graph("baba.txt", "baba.txt");

            using (var stream = GenerateStreamFromString(BasicVericesWeights))
            {
                graph.ReadVerticesWeights(stream);
            }

            Assert.AreEqual(9, graph.VerticesWeights.Count);
        }
        
        [TestMethod]
        public void ReadEdgesWeights_CorrectFormat_Success()
        {
            var graph = new Graph("baba.txt", "baba.txt");
            graph.VerticesWeights = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9};
            
            using (var stream = GenerateStreamFromString(BasicEdgesWeights))
            {
                graph.ReadEdgesWeights(stream);
            }

            Assert.AreEqual(3, graph.EdgesWeights[0,1]);
        }

        [TestMethod]
        public void InitializePheromoneMatrix_AllInitialized_Success()
        {
            var graph = new Graph("baba.txt", "baba.txt");
            graph.VerticesWeights = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            graph.InitializePheromoneMatrix();

            Assert.AreEqual(Constants.MinimalValueOfPheromone, graph.PheromoneMatrix[0,1]);
        }

        private static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}