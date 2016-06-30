using System;
using AlgorithmsCore;
using AlgorithmsCore.Contracts;
using AlgorithmsCore.Options;

namespace ParallelOptimisation
{
    public class AspgParallelOptimisation : AspgBase
    {
        public AspgParallelOptimisation(OptionsParallelOptimisation options, IGraph graph, Random rnd)
            : base (options, graph, rnd) { }

        public override ResultData GetQuality()
        {
            var bestResult = new Result(double.MinValue);

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
                        decimal[] probability = antSystem.CalculateProbability(interSectionIndex, nextColony);
                        var chosenVertexIndex = Roulette(probability);
                        var chosenVertex = Graph.VerticesWeights[chosenVertexIndex];
                        antSystem.AddFreeVertexToTreil(interSectionIndex, nextColony, chosenVertex);
                    }
                }

                var bestFragment = antSystem.UpdatePhermone();

                var newQuality = bestFragment.SumOfOptimalityCriterion;
                // Save the best results.
                if (bestResult.Quality < newQuality)
                {
                    bestResult = new Result(newQuality, bestFragment.Treil);
                }

                Options.NumberOfIterations--;
            }

            //return bestResult;
            return null;
        }
    }
}
