using System;
using System.Linq;
using AlgorithmsCore;

namespace ParallelOptimisation
{
    public class AspgParallelOptimisation
    {
        private Options _options;
        private readonly IGraph _graph;
        private readonly Random _rnd;

        public AspgParallelOptimisation(Options options, IGraph graph, Random rnd)
        {
            _options = options;
            _graph = graph;
            _rnd = rnd;
        }

        public Result GetQuality()
        {
            var result = new Result(double.MinValue);
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
                if (result.Quality < sumOfOptimalityCriterions)
                {
                    result = new Result(sumOfOptimalityCriterions, antSystem.Treil);
                }

                _options.NumberOfIterations--;
            }

            return result;
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
