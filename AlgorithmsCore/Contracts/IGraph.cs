using System.Collections.Generic;

namespace AlgorithmsCore.Contracts
{
    public interface IGraph
    {
        List<Vertex> VerticesWeights { get; }

        int[,] EdgesWeights { get;  }

        double[,] PheromoneMatrix { get; }

        int NumberOfVertices { get; }

        void UpdatePhermone(AntSystemFragment antSystemFragment, Options.Options options,
            double sumOfOptimalityCriterions);
    }
}
