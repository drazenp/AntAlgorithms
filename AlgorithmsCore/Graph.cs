using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AlgorithmsCore.Contracts;

namespace AlgorithmsCore
{
    public class Graph : IGraph
    {
        private readonly string _verticesWeigtFilePath;
        private readonly string _edgesWeighFilePath;

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

        public Graph(string verticesWeigtFilePath, string edgesWeighFilePath)
        {
            _verticesWeigtFilePath = verticesWeigtFilePath;
            _edgesWeighFilePath = edgesWeighFilePath;
        }

        public void InitializeGraph()
        {
            if (!File.Exists(_verticesWeigtFilePath))
            {
                throw new FileNotFoundException("The file path with vertices weights doesn't exisits.");
            }

            if (!File.Exists(_edgesWeighFilePath))
            {
                throw new FileNotFoundException("The file path with edges weights doesn't exisits.");
            }

            using (var straem = File.OpenRead(_verticesWeigtFilePath))
            {
                ReadVerticesWeights(straem);
            }

            NumberOfVertices = VerticesWeights.Count;

            using (var straem = File.OpenRead(_edgesWeighFilePath))
            {
                ReadEdgesWeights(straem);
            }

            InitializePheromoneMatrix();
        }

        public void ReadVerticesWeights(Stream straem)
        {
            using (var reader = new StreamReader(straem))
            {
                // Skip first line.
                var line = reader.ReadLine();
                var vertexIndex = 0;
                while (line != null)
                {
                    line = reader.ReadLine();

                    // TODO: Check what to do where. -> exceptions?
                    if (line == null) continue;

                    var vertex = new Vertex(vertexIndex, int.Parse(line));
                    vertexIndex++;
                    VerticesWeights.Add(vertex);
                }
            }
        }

        public void ReadEdgesWeights(Stream straem)
        {
            if (NumberOfVertices <= 1)
                throw new Exception("Number of vertices must be greter than 1.");

            EdgesWeights = new int[NumberOfVertices, NumberOfVertices];

            using (var reader = new StreamReader(straem))
            {
                // Skip first line.
                var line = reader.ReadLine();
                while (line != null)
                {
                    line = reader.ReadLine();

                    // TODO: Check what to do where. -> exceptions?
                    if (line == null) continue;

                    var fileData = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    EdgesWeights[int.Parse(fileData[0]), int.Parse(fileData[1])] = int.Parse(fileData[2]);
                    EdgesWeights[int.Parse(fileData[1]), int.Parse(fileData[0])] = int.Parse(fileData[2]);
                }
            }
        }

        public void InitializePheromoneMatrix()
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

        public void UpdatePhermone(int[] weightOfColonies, List<HashSet<Vertex>> treil, Options.Options options, double sumOfOptimalityCriterions)
        {
            // Colony with heighes weight.
            var colonyWithHighestWeight = Array.IndexOf(weightOfColonies, weightOfColonies.Max());

            // If criterions of optimality is less then 0, the minimum of pheromones will be set.
            if (sumOfOptimalityCriterions > 0)
            {
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

                    var path = treil[indexOfRegion];

                    var verexCombination = path.SelectMany((value, index) => path.Skip(index + 1),
                               (first, second) => new { first, second });

                    foreach (var combination in verexCombination)
                    {
                        PheromoneMatrix[combination.first.Index, combination.second.Index] = PheromoneMatrix[combination.first.Index, combination.second.Index] * (1 - options.Ro) + pheromoneToSet;
                        PheromoneMatrix[combination.second.Index, combination.first.Index] = PheromoneMatrix[combination.second.Index, combination.first.Index] * (1 - options.Ro) + pheromoneToSet;
                    }
                }
            }
            else
            {
                for (var i = 0; i < PheromoneMatrix.GetLength(0); i++)
                {
                    for (var j = 0; j < PheromoneMatrix.GetLength(1); j++)
                    {
                        PheromoneMatrix[i, j] = PheromoneMatrix[i, j] * (1 - options.Ro) + Constants.MinimalVelueOfPheromoneToSet;
                    }
                }
            }

            Utility.LogDoubleMatrixAsTable(PheromoneMatrix);
        }

        /*
        public double GetUpdatedPheromoneOnOneEdge(int firstIndex , int secondIndex, double evaporation)
        {
            var currentPheromoneValue = PheromoneMatrix[firstIndex, secondIndex];
            var newPheromoneValue = currentPheromoneValue*(1 - evaporation);
            if (newPheromoneValue < 0.0001)
            {
                return 0.0001;
            }
            return newPheromoneValue;
        }
        */
    }
}
