using System;
using System.Collections.Generic;
using System.Linq;

namespace Basic
{
    public class Aspg
    {
        private Options _options;
        private readonly Graph _graph;

        public Aspg(Options options, Graph graph)
        {
            _options = options;
            _graph = graph;
        }

        public decimal GetQuality()
        {
            decimal quality = Decimal.MinValue;

            while (_options.NumberOfIterations > 0)
            {

            }

            return quality;
        }

        public decimal GetMaxAllowedWeight(int[] verticesWeights)
        {
            var sumOfVerticesWeightes = verticesWeights.Sum();
            decimal maxAllowedWeight = sumOfVerticesWeightes / _options.NumberOfRegions * (1 + _options.Delta);
            return maxAllowedWeight;
        }
    }
}
