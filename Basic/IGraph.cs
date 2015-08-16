using System.Collections.Generic;

namespace Basic
{
    public interface IGraph
    {
        List<int> VerticesWeights { get; set; }

        int[,] EdgesWeights { get; }

        double[,] PheromoneMatrix { get; }

        int NumberOfVertices { get; }
    }
}
