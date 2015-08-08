using System;
using System.Collections.Generic;
using System.IO;

namespace Basic
{
    public class Graph
    {
        private readonly string _verticesWeigtFilePath;
        private readonly string _edgesWeighFilePath;

        public List<int> VerticesWeights { get; private set; } = new List<int>();
        public List<int> EdgesWeights { get; private set; }

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

            using (var straem = File.OpenRead(_edgesWeighFilePath))
            {
                ReadEdgesWeights(straem);
            }
        }

        public void ReadVerticesWeights(Stream straem)
        {
            using (var reader = new StreamReader(straem))
            {
                var line = reader.ReadLine();
                while (line != null)
                {
                    line = reader.ReadLine();

                    // TODO: Check what to do where. -> exceptions?
                    if (line == null) continue;

                    VerticesWeights.Add(int.Parse(line));
                }
            }
        }

        private void ReadEdgesWeights(Stream straem)
        {
            using (var reader = new StreamReader(straem))
            {
            }
        }
    }
}
