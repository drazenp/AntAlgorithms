using System;
using System.Linq;
using AlgorithmsCore;

namespace ParallelOptimisation
{
    public class AspgParallelOptimisation
    {
        private OptionsParallelOptimisation _options;
        private readonly IGraph _graph;
        private readonly Random _rnd;

        public AspgParallelOptimisation(OptionsParallelOptimisation options, IGraph graph, Random rnd)
        {
            _options = options;
            _graph = graph;
            _rnd = rnd;
        }

        public Result GetQuality()
        {
            var bestResult = new Result(double.MinValue);
            var maxAllowedWeight = GetMaxAllowedWeight();

            while (_options.NumberOfIterations > 0)
            {
                var antSystem = new AntSystemParallelOptimisation(_rnd, _options, _graph);

                for (short interSectionIndex = 0;
                    interSectionIndex < _options.NumberOfInterSections;
                    interSectionIndex++)
                {
                    for (var vertexIndex = _options.NumberOfRegions;
                        vertexIndex < _graph.NumberOfVertices;
                        vertexIndex++)
                    {
                        var nextColony = antSystem.GetNextColony(interSectionIndex);
                        double[] probability = antSystem.CalculateProbability(interSectionIndex, nextColony);
                        var chosenVertexIndex = Roulette(probability);
                        var chosenVertex = _graph.VerticesWeights[chosenVertexIndex];
                        antSystem.AddFreeVertexToTreil(interSectionIndex, nextColony, chosenVertex);
                    }
                }

                antSystem.UpdatePhermone(maxAllowedWeight);

                // Save the best results.
                if (bestResult.Quality < antSystem.FragmentBestOptimalityCriterion)
                {
                    bestResult = new Result(antSystem.FragmentBestOptimalityCriterion, antSystem.BestTreil);
                }

                _options.NumberOfIterations--;
            }

            return bestResult;
        }

        public double GetMaxAllowedWeight()
        {
            var sumOfVerticesWeightes = _graph.VerticesWeights.Select(v => v.Weight).Sum();
            double maxAllowedWeight = sumOfVerticesWeightes / (double)_options.NumberOfRegions * (1 + _options.Delta);
            return maxAllowedWeight;
        }

        // TODO: Consider moving ths function in some kind of utility class.
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
