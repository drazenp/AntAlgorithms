using System;
using System.Linq;
using AlgorithmsCore;

namespace ParallelOptimisation
{
    public class AspgParallelOptimisation : AspgBase
    {
        public AspgParallelOptimisation(OptionsParallelOptimisation options, IGraph graph, Random rnd)
            : base (options, graph, rnd) { }

        public override Result GetQuality()
        {
            var bestResult = new Result(double.MinValue);
            var maxAllowedWeight = GetMaxAllowedWeight();

            var options = (OptionsParallelOptimisation) Options;
            while (Options.NumberOfIterations > 0)
            {
                var antSystem = new AntSystemParallelOptimisation(Rnd, options, Graph);

                for (short interSectionIndex = 0;
                    interSectionIndex < options.NumberOfInterSections;
                    interSectionIndex++)
                {
                    for (var vertexIndex = Options.NumberOfRegions;
                        vertexIndex < Graph.NumberOfVertices;
                        vertexIndex++)
                    {
                        var nextColony = antSystem.GetNextColony(interSectionIndex);
                        double[] probability = antSystem.CalculateProbability(interSectionIndex, nextColony);
                        var chosenVertexIndex = Roulette(probability);
                        var chosenVertex = Graph.VerticesWeights[chosenVertexIndex];
                        antSystem.AddFreeVertexToTreil(interSectionIndex, nextColony, chosenVertex);
                    }
                }

                antSystem.UpdatePhermone(maxAllowedWeight);

                // Save the best results.
                if (bestResult.Quality < antSystem.FragmentBestOptimalityCriterion)
                {
                    bestResult = new Result(antSystem.FragmentBestOptimalityCriterion, antSystem.BestTreil);
                }

                Options.NumberOfIterations--;
            }

            return bestResult;
        }
    }
}
