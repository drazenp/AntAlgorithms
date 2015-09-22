using System.Collections.Generic;

namespace AlgorithmsCore
{
    public interface IGraph
    {
        List<Vertex> VerticesWeights { get; }

        int[,] EdgesWeights { get;  }

        double[,] PheromoneMatrix { get; }

        int NumberOfVertices { get; }
    }
}
