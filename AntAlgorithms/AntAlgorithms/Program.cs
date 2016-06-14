using System;
using System.CodeDom;
using System.Reflection;
using AlgorithmsCore;
using AlgorithmsCore.Options;
using log4net;
using log4net.Config;
using ParallelOptimisation;
using ParallelOptimisationWithInheritance;

namespace AntAlgorithms
{
    static class Program
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string BasicEdges = "Graphs/B2.txt";
        private const string BasicVertexWeights = "Graphs/B1.txt";
        private const string AdvancedEdges = "Graphs/54_1.txt";
        private const string AdvancedVertexWeights = "Graphs/54_2.txt";

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            var rnd = new Random(Environment.TickCount);

            //var options = new Options(numberOfIterations: 10, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
            //var graph = new Graph(BasicEdges, BasicVertexWeights);
            //graph.InitializeGraph();
            //var aspg = new Aspg(options, graph, rnd);
            //var resultBasic = aspg.GetQuality();

            var parallelOptimisationOptoins =
                new OptionsParallelOptimisation(numberOfIterations: 10, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D, numberOfInterSections: 5);
            var graph = new Graph(AdvancedVertexWeights, AdvancedEdges);
            graph.InitializeGraph();
            var aspgParallelOptimisation = new AspgParallelOptimisation(parallelOptimisationOptoins, graph, rnd);
            var resultParallelOptimisation = aspgParallelOptimisation.GetQuality();

            //var parallelOptimisationWithInheritanceOptoins =
            //    new OptionsParallelOptimisation(numberOfIterations: 10, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D, numberOfInterSections: 5);
            //var graph = new Graph(AdvancedVertexWeights, AdvancedEdges);
            //graph.InitializeGraph();
            //var aspgParallelOptimisationWithInheritance = new AspgParallelOptimisationWithInheritance(parallelOptimisationWithInheritanceOptoins,
            //    graph, rnd);
            //var resultParallelOptimisationWithInheritance = aspgParallelOptimisationWithInheritance.GetQuality();
        }
    }
}
