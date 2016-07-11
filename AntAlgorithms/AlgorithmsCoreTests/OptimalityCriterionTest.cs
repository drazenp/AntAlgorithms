using System.Collections.Generic;
using System.Fakes;
using AlgorithmsCore;
using AlgorithmsCore.Contracts;
using AlgorithmsCore.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AlgorithmsCoreTests
{
    [TestClass]
    public class OptimalityCriterionTest
    {
        #region Myciel4
        private readonly List<string> _myciel4 = new List<string>() { "p edges 23 71",
                                                                      "e 1 2",
                                                                      "e 1 4",
                                                                      "e 1 7",
                                                                      "e 1 9",
                                                                      "e 1 13",
                                                                      "e 1 15",
                                                                      "e 1 18",
                                                                      "e 1 20",
                                                                      "e 2 3",
                                                                      "e 2 6",
                                                                      "e 2 8",
                                                                      "e 2 12",
                                                                      "e 2 14",
                                                                      "e 2 17",
                                                                      "e 2 19",
                                                                      "e 3 5",
                                                                      "e 3 7",
                                                                      "e 3 10",
                                                                      "e 3 13",
                                                                      "e 3 16",
                                                                      "e 3 18",
                                                                      "e 3 21",
                                                                      "e 4 5",
                                                                      "e 4 6",
                                                                      "e 4 10",
                                                                      "e 4 12",
                                                                      "e 4 16",
                                                                      "e 4 17",
                                                                      "e 4 21",
                                                                      "e 5 8",
                                                                      "e 5 9",
                                                                      "e 5 14",
                                                                      "e 5 15",
                                                                      "e 5 19",
                                                                      "e 5 20",
                                                                      "e 6 11",
                                                                      "e 6 13",
                                                                      "e 6 15",
                                                                      "e 6 22",
                                                                      "e 7 11",
                                                                      "e 7 12",
                                                                      "e 7 14",
                                                                      "e 7 22",
                                                                      "e 8 11",
                                                                      "e 8 13",
                                                                      "e 8 16",
                                                                      "e 8 22",
                                                                      "e 9 11",
                                                                      "e 9 12",
                                                                      "e 9 16",
                                                                      "e 9 22",
                                                                      "e 10 11",
                                                                      "e 10 14",
                                                                      "e 10 15",
                                                                      "e 10 22",
                                                                      "e 11 17",
                                                                      "e 11 18",
                                                                      "e 11 19",
                                                                      "e 11 20",
                                                                      "e 11 21",
                                                                      "e 12 23",
                                                                      "e 13 23",
                                                                      "e 14 23",
                                                                      "e 15 23",
                                                                      "e 16 23",
                                                                      "e 17 23",
                                                                      "e 18 23",
                                                                      "e 19 23",
                                                                      "e 20 23",
                                                                      "e 21 23",
                                                                      "e 22 23"};
        #endregion
        
        [TestMethod]
        public void DimacsMyciel4_GetSumOfOptimalityCriterion_34()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_myciel4);

            var graph = new DimacsGraph(loaderMock.Object);
            graph.InitializeGraph();

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var options = new BaseOptions(numberOfIterations: 100, numberOfRegions: 2, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);

            var fragment = new UnweightedAntSystemFragmentFake(randomMock, options, graph);
            fragment.ClearTreil();
            fragment.Treil[0].Add(new Vertex(4, 1));
            fragment.Treil[0].Add(new Vertex(8, 1));
            fragment.Treil[0].Add(new Vertex(9, 1));
            fragment.Treil[0].Add(new Vertex(11, 1));
            fragment.Treil[0].Add(new Vertex(12, 1));
            fragment.Treil[0].Add(new Vertex(14, 1));
            fragment.Treil[0].Add(new Vertex(17, 1));
            fragment.Treil[0].Add(new Vertex(18, 1));
            fragment.Treil[0].Add(new Vertex(19, 1));
            fragment.Treil[0].Add(new Vertex(21, 1));
            fragment.Treil[0].Add(new Vertex(22, 1));

            fragment.Treil[1].Add(new Vertex(0, 1));
            fragment.Treil[1].Add(new Vertex(1, 1));
            fragment.Treil[1].Add(new Vertex(2, 1));
            fragment.Treil[1].Add(new Vertex(3, 1));
            fragment.Treil[1].Add(new Vertex(5, 1));
            fragment.Treil[1].Add(new Vertex(6, 1));
            fragment.Treil[1].Add(new Vertex(7, 1));
            fragment.Treil[1].Add(new Vertex(10, 1));
            fragment.Treil[1].Add(new Vertex(13, 1));
            fragment.Treil[1].Add(new Vertex(15, 1));
            fragment.Treil[1].Add(new Vertex(16, 1));
            fragment.Treil[1].Add(new Vertex(20, 1));

            var globalCost = fragment.SumOfOptimalityCriterion;

            Assert.AreEqual(34, globalCost);
        }

        [TestMethod]
        public void DimacsMyciel4_GetSumOfOptimalityCriterion_28()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_myciel4);

            var graph = new DimacsGraph(loaderMock.Object);
            graph.InitializeGraph();

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var options = new BaseOptions(numberOfIterations: 100, numberOfRegions: 2, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);

            var fragment = new UnweightedAntSystemFragmentFake(randomMock, options, graph);
            fragment.ClearTreil();
            fragment.Treil[0].Add(new Vertex(3, 1));
            fragment.Treil[0].Add(new Vertex(16, 1));
            fragment.Treil[0].Add(new Vertex(9, 1));
            fragment.Treil[0].Add(new Vertex(10, 1));
            fragment.Treil[0].Add(new Vertex(5, 1));
            fragment.Treil[0].Add(new Vertex(21, 1));
            fragment.Treil[0].Add(new Vertex(8, 1));
            fragment.Treil[0].Add(new Vertex(7, 1));
            fragment.Treil[0].Add(new Vertex(15, 1));
            fragment.Treil[0].Add(new Vertex(6, 1));
            fragment.Treil[0].Add(new Vertex(11, 1));
            fragment.Treil[0].Add(new Vertex(20, 1));

            fragment.Treil[1].Add(new Vertex(22, 1));
            fragment.Treil[1].Add(new Vertex(19, 1));
            fragment.Treil[1].Add(new Vertex(12, 1));
            fragment.Treil[1].Add(new Vertex(0, 1));
            fragment.Treil[1].Add(new Vertex(17, 1));
            fragment.Treil[1].Add(new Vertex(14, 1));
            fragment.Treil[1].Add(new Vertex(2, 1));
            fragment.Treil[1].Add(new Vertex(4, 1));
            fragment.Treil[1].Add(new Vertex(1, 1));
            fragment.Treil[1].Add(new Vertex(13, 1));
            fragment.Treil[1].Add(new Vertex(18, 1));

            var globalCost = fragment.SumOfOptimalityCriterion;

            Assert.AreEqual(28, globalCost);
        }
    }
}
