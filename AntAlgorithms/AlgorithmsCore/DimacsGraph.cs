using AlgorithmsCore.Contracts;
using System;
using System.Collections.Generic;
using AlgorithmsCore.Options;
using System.Linq;

namespace AlgorithmsCore
{
    public class DimacsGraph : IGraph
    {
        private readonly IDataLoader _dataLoader;

        public List<Vertex> VerticesWeights { get; set; } = new List<Vertex>();

        public int[,] EdgesWeights { get; private set; }

        public double[,] PheromoneMatrix { get; private set; }

        private int _numberOfVertices;
        public int NumberOfVertices
        {
            get
            {
                return _numberOfVertices != default(int) ? _numberOfVertices : VerticesWeights.Count;
            }
            private set { _numberOfVertices = value; }
        }

        // TODO: Find a way to define if edges are double calculated or not.
        public int NumberOfEdges { get; set; }

        public DimacsGraph(IDataLoader dataLoader)
        {
            _dataLoader = dataLoader;
        }

        public void InitializeGraph()
        {
            ReadGraphData();

            NumberOfVertices = VerticesWeights.Count;

            InitializePheromoneMatrix();
        }

        private void ReadGraphData()
        {
            foreach (var line in _dataLoader.LoadData())
            {
                var fileData = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                switch (fileData[0])
                {
                    case "p":
                        var numberOfVertices = int.Parse(fileData[2]);
                        for (var i = 0; i < numberOfVertices; i++)
                        {
                            VerticesWeights.Add(new Vertex(i));
                        }

                        EdgesWeights = new int[NumberOfVertices, NumberOfVertices];

                        NumberOfEdges = int.Parse(fileData[3]);
                        break;
                    case "e":
                        var vertexID = int.Parse(fileData[1]) - 1;
                        var connectedVertexID = int.Parse(fileData[2]) - 1;

                        EdgesWeights[vertexID, connectedVertexID] = 1;
                        EdgesWeights[connectedVertexID, vertexID] = 1;
                        break;
                }
            }
        }

        private void InitializePheromoneMatrix()
        {
            PheromoneMatrix = new double[NumberOfVertices, NumberOfVertices];
            for (var i = 0; i < NumberOfVertices; i++)
            {
                for (var j = 0; j < NumberOfVertices; j++)
                {
                    if (i == j) continue;

                    PheromoneMatrix[i, j] = Constants.MinimalValueOfPheromone;
                }
            }
        }

        public void UpdatePhermone(int[] weightOfColonies, List<HashSet<Vertex>> treil, BaseOptions options, double sumOfOptimalityCriterions)
        {
            // Colony with heighes weight.
            var colonyWithHighestWeight = Array.IndexOf(weightOfColonies, weightOfColonies.Max());

            for (var indexOfRegion = 0; indexOfRegion < options.NumberOfRegions; indexOfRegion++)
            {
                double pheromoneToSet;
                if (indexOfRegion == colonyWithHighestWeight)
                {
                    pheromoneToSet = 0.01D * (sumOfOptimalityCriterions + 1.2D * weightOfColonies[colonyWithHighestWeight]);
                }
                else
                {
                    pheromoneToSet = 0.01D * sumOfOptimalityCriterions;
                }

                for (int i = 0; i < NumberOfVertices; i++)
                {
                    for (int j = 0; j < NumberOfVertices; j++)
                    {
                        PheromoneMatrix[i, j] = PheromoneMatrix[i, j]*(1 - options.Ro);
                    }
                }

                var path = treil[indexOfRegion];

                var verexCombination = path.SelectMany((value, index) => path.Skip(index + 1),
                            (first, second) => new { first, second });

                foreach (var combination in verexCombination.Where(comb => EdgesWeights[comb.first.Index, comb.second.Index] == 1))
                {
                    PheromoneMatrix[combination.first.Index, combination.second.Index] = PheromoneMatrix[combination.first.Index, combination.second.Index] + pheromoneToSet;
                    PheromoneMatrix[combination.second.Index, combination.first.Index] = PheromoneMatrix[combination.second.Index, combination.first.Index] + pheromoneToSet;
                }
            }
        }
    }
}
