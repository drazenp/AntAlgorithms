using AlgorithmsCore.Options;
using System.Collections.Generic;
using System.Reflection;
using log4net;

namespace AlgorithmsCore.Contracts
{
    public interface IGraph
    {
        List<Vertex> VerticesWeights { get; }

        int[,] EdgesWeights { get;  }

        double[,] PheromoneMatrix { get; }

        int NumberOfVertices { get; }

        int NumberOfEdges { get; }

        void InitializeGraph();

        void UpdatePhermone(int[] weightOfColonies, List<HashSet<Vertex>> treil, BaseOptions options,
            double sumOfOptimalityCriterions);
    }
}
