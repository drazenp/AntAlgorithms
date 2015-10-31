using System;
using AlgorithmsCore;
using AlgorithmsCore.Options;
using Basic;
using ParallelOptimisation;
using ParallelOptimisationWithInheritance;

namespace AntAlgorithms
{
    static class Program
    {
        private const string BasicEdges = "Graphs/B2.txt";
        private const string BasicVertexWeights = "Graphs/B1.txt";

        static void Main(string[] args)
        {
            var options = new Options(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
            var graph = new Graph(BasicEdges, BasicVertexWeights);
            graph.InitializeGraph();
            var rnd = new Random(Environment.TickCount);
            var aspg = new Aspg(options, graph, rnd);
            var resultBasic = aspg.GetQuality();

            var parallelOptimisationOptoins = 
                new OptionsParallelOptimisation(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D, numberOfInterSections:5);
            graph = new Graph(BasicEdges, BasicVertexWeights);
            graph.InitializeGraph();
            var aspgParallelOptimisation = new AspgParallelOptimisation(parallelOptimisationOptoins, graph, rnd);
            var resultParallelOptimisation = aspgParallelOptimisation.GetQuality();

            graph = new Graph(BasicEdges, BasicVertexWeights);
            graph.InitializeGraph();
            var aspgParallelOptimisationWithInheritance = new AspgParallelOptimisationWithInheritance(parallelOptimisationOptoins,
                graph, rnd);
            var resultParallelOptimisationWithInheritance = aspgParallelOptimisationWithInheritance.GetQuality();
        }
    }
}
