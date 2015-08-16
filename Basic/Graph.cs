using System;
using System.Collections.Generic;
using System.IO;

namespace Basic
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
                }
            }
        }

        public void InitializePheromoneMatrix()
        {
            PheromoneMatrix = new double[NumberOfVertices, NumberOfVertices];
            for (int i = 0; i < NumberOfVertices - 1; i++)
            {
                for (var j = 0; j < NumberOfVertices - 1; j++)
                {
                    if (i == j) continue;

                    PheromoneMatrix[i, j] = Constants.MinimalValueOfPheromone;
                }
            }
        }
    }
}
