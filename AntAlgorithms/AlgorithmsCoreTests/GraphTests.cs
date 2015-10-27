﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AlgorithmsCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgorithmsCoreTests
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
        public void ReadVerticesWeights_CorrectCount_Success()
        {
            var graph = new Graph("baba.txt", "baba.txt");

            using (var stream = GenerateStreamFromString(BasicVericesWeights))
            {
                graph.ReadVerticesWeights(stream);
            }

            Assert.AreEqual(9, graph.VerticesWeights.Count);
        }

        [TestMethod]
        public void ReadVerticesWeights_CorrectVertexFormat_Success()
        {
            var graph = new Graph("baba.txt", "baba.txt");

            using (var stream = GenerateStreamFromString(BasicVericesWeights))
            {
                graph.ReadVerticesWeights(stream);
            }

            var duplicates = graph.VerticesWeights.GroupBy(x => x.Index)
                                  .Where(g => g.Count() > 1)
                                  .Select(y => y.Key)
                                  .ToList();

            Assert.AreEqual(0, duplicates.Count);
        }

        [TestMethod]
        public void ReadEdgesWeights_CorrectFormat_Success()
        {
            var graph = new Graph("baba.txt", "baba.txt");
            graph.VerticesWeights = new List<Vertex>
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

            using (var stream = GenerateStreamFromString(BasicEdgesWeights))
            {
                graph.ReadEdgesWeights(stream);
            }

            Assert.AreEqual(3, graph.EdgesWeights[0, 1]);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Number of vertices is not greter than 1")]
        public void ReadEdgesWeights_CorrectFormat_Fail()
        {
            var graph = new Graph("baba.txt", "baba.txt");

            using (var stream = GenerateStreamFromString(BasicEdgesWeights))
            {
                graph.ReadEdgesWeights(stream);
            }

            Assert.AreEqual(3, graph.EdgesWeights[0, 1]);
        }

        [TestMethod]
        public void InitializePheromoneMatrix_AllInitialized_Success()
        {
            var graph = new Graph("baba.txt", "baba.txt");
            graph.VerticesWeights = new List<Vertex>
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

            graph.InitializePheromoneMatrix();

            Assert.AreEqual(Constants.MinimalValueOfPheromone, graph.PheromoneMatrix[0, 1]);
        }

        [TestMethod]
        public void InitializePheromoneMatrix_DiagonalZeros_Success()
        {
            var graph = new Graph("baba.txt", "baba.txt");
            graph.VerticesWeights = new List<Vertex>
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

            graph.InitializePheromoneMatrix();

            Assert.AreEqual(0, graph.PheromoneMatrix[0, 0]);
            Assert.AreEqual(0, graph.PheromoneMatrix[1, 1]);
            Assert.AreEqual(0, graph.PheromoneMatrix[2, 2]);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException), "Wrong file path for vertices weight was inappropriately allowed.")]
        public void InitializeGraph_VerticesWeigtFilePath_NotExisits_Fail()
        {
            var graph = new Graph("baba.txt", "baba.txt");

            graph.InitializeGraph();
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException), "Wrong file path for edges weight was inappropriately allowed.")]
        public void InitializeGraph_EdgesWeighFilePath_NotExisits_Fail()
        {
            var graph = new Graph(System.Reflection.Assembly.GetExecutingAssembly().Location, "baba.txt");

            graph.InitializeGraph();
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