using System;
using System.Linq;

namespace Basic
{
    public class Aspg
    {
        private Options _options;
        private readonly Graph _graph;

        private Random _rnd = new Random(Environment.TickCount);
        
        public Aspg(Options options, Graph graph)
        {
            _options = options;
            _graph = graph;
        }

        public decimal GetQuality()
        {
            decimal quality = decimal.MinValue;
            int maxNumberOfVerticesInTrail = _graph.NumberOfVertices - _options.NumberOfRegions + 1;

            while (_options.NumberOfIterations > 0)
            {
                // niz zauzetih tacaka
                int[] zauzeteTacke = new int[_graph.NumberOfVertices];
                // prati broj cvorova u regionima
                int[,] korak = new int[_options.NumberOfRegions, 1];
                //ASPGOpcije.Korak = zeros(ASPGOpcije.h, 1); % prati broj cvorova u regionima



                _options.NumberOfIterations--;
            }

            return quality;
        }

        public decimal GetMaxAllowedWeight(int[] verticesWeights)
        {
            var sumOfVerticesWeightes = verticesWeights.Sum();
            decimal maxAllowedWeight = sumOfVerticesWeightes / (decimal)_options.NumberOfRegions * (1 + _options.Delta);
            return maxAllowedWeight;
        }
    }
}
