using System;
using System.Collections.Generic;
using System.Linq;
using AlgorithmsCore;

namespace Basic
{
    public class Aspg
    {
        private Options _options;
        private readonly IGraph _graph;
        private readonly Random _rnd;

        private List<HashSet<Vertex>> _bestTrail;
        private double _bestOptimalityCriterions = double.MinValue; 

        public Aspg(Options options, IGraph graph, Random rnd)
        {
            _options = options;
            _graph = graph;
            _rnd = rnd;
        }

        public decimal GetQuality()
        {
            decimal quality = decimal.MinValue;
            var maxAllowedWeight = GetMaxAllowedWeight();

            while (_options.NumberOfIterations > 0)
            {
                var antSystem = new AntSystem(_rnd, _options, _graph);
                antSystem.InitializeTreils();

                for (var vertexIndex = _options.NumberOfRegions; vertexIndex < _graph.NumberOfVertices; vertexIndex++)
                {
                    var nextColony = antSystem.GetNextColony();
                    double[] probability = antSystem.CalculateProbability(nextColony);
                    var chosenVertexIndex = Roulette(probability);
                    var chosenVertex = _graph.VerticesWeights[chosenVertexIndex];
                    antSystem.AddFreeVertexToTreil(nextColony, chosenVertex);
                }

                var sumOfOptimalityCriterions = antSystem.UpdatePhermone(maxAllowedWeight);

                // Save the best results.
                if (_bestOptimalityCriterions < sumOfOptimalityCriterions)
                {
                    _bestOptimalityCriterions = sumOfOptimalityCriterions;
                    _bestTrail = antSystem.GetCopyOfTrails();
                }

                _options.NumberOfIterations--;
            }

            return quality;
        }

        public double GetMaxAllowedWeight()
        {
            var sumOfVerticesWeightes = _graph.VerticesWeights.Select(v => v.Weight).Sum();
            double maxAllowedWeight = sumOfVerticesWeightes / (double)_options.NumberOfRegions * (1 + _options.Delta);
            return maxAllowedWeight;
        }

        public int Roulette(double[] probability)
        {
            var boundary = _rnd.NextDouble();
            var currentSumOfProbability = 0D;
            for (int i = 0; i < _graph.NumberOfVertices; i++)
            {
                currentSumOfProbability += probability[i];
                if (boundary <= currentSumOfProbability)
                {
                    return i;
                }
            }

            return _graph.NumberOfVertices - 1;
        }
    }
}
