using System;
using AlgorithmsCore;
using AlgorithmsCore.Contracts;
using AlgorithmsCore.Options;

namespace ParallelOptimisationWithInheritance
{
    public class AspgParallelOptimisationWithInheritance : AspgBase
    {
        public AspgParallelOptimisationWithInheritance(OptionsParallelOptimisation options, IGraph graph, Random rnd)
            : base (options, graph, rnd) { }

        public override Result GetQuality()
        {
            var bestResult = new Result(double.MinValue);

            AntSystemFragment previousBestFragment = null;

            var options = (OptionsParallelOptimisation)Options;
            while (Options.NumberOfIterations > 0)
            {
                var antSystem = new AntSystemParallelOptimisationWithInheritance(Rnd, options, Graph, previousBestFragment);

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

                previousBestFragment = antSystem.UpdatePhermone();

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
