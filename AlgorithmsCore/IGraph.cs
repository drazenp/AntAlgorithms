using System.Collections.Generic;
using AlgorithmsCore;

namespace AlgorithmsCore
{
    public interface IGraph
    {
        List<Vertex> VerticesWeights { get; set; }

        int[,] EdgesWeights { get; }

        double[,] PheromoneMatrix { get; }

        int NumberOfVertices { get; }
    }
}
