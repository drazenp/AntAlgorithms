using System;
using System.Collections.Generic;
using AlgorithmsCore.Contracts;

namespace AlgorithmsCore
{
    public class GraphWithLoader : IGraph
    {
        public List<Vertex> VerticesWeights { get; }
        public int[,] EdgesWeights { get; }
        public double[,] PheromoneMatrix { get; }
        public int NumberOfVertices { get; }
        public int NumberOfEdges { get; }

        public void UpdatePhermone(int[] weightOfColonies, List<HashSet<Vertex>> treil, Options.BaseOptions options, double sumOfOptimalityCriterions)
        {
            throw new NotImplementedException();
        }

        public void InitializeGraph()
        {
            throw new NotImplementedException();
        }
    }
}
