using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgorithmsCore.Contracts;

namespace AlgorithmsCore
{
    public class GraphWithLoader : IGraph
    {
        public List<Vertex> VerticesWeights { get; }
        public int[,] EdgesWeights { get; }
        public double[,] PheromoneMatrix { get; }
        public int NumberOfVertices { get; }
        public void UpdatePhermone(int[] weightOfColonies, List<HashSet<Vertex>> treil, Options.Options options, double sumOfOptimalityCriterions)
        {
            throw new NotImplementedException();
        }
    }
}
