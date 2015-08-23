﻿using System;
using Basic;

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
            aspg.GetQuality();
        }
    }
}
