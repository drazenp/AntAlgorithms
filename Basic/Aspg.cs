using System;
using AlgorithmsCore;
using AlgorithmsCore.Contracts;
using AlgorithmsCore.Options;

namespace Basic
{
    public class Aspg : AspgBase
    {
        public Aspg(Options options, IGraph graph, Random rnd)
            : base(options, graph, rnd)
        { }

        public override Result GetQuality()
        {
            var result = new Result(double.MinValue);
            var maxAllowedWeight = GetMaxAllowedWeight();

            while (Options.NumberOfIterations > 0)
            {
                Log.Debug("Iteration: " + Options.NumberOfIterations);
                var antSystem = new AntSystemBasic(Rnd, Options, Graph);

                for (var vertexIndex = Options.NumberOfRegions; vertexIndex < Graph.NumberOfVertices; vertexIndex++)
                {
                    var nextColony = antSystem.GetNextColony();
                    decimal[] probability = antSystem.CalculateProbability(nextColony);
                    var chosenVertexIndex = Roulette(probability);
                    var chosenVertex = Graph.VerticesWeights[chosenVertexIndex];
                    antSystem.AddFreeVertexToTreil(nextColony, chosenVertex);
                }

                var bestFragment = antSystem.UpdatePhermone(maxAllowedWeight);

                var newQuality = bestFragment.GetSumOfOptimalityCriterion(maxAllowedWeight);
                Log.Debug($"New quality: {newQuality}");

                // Save the best results.
                if (result.Quality < newQuality)
                {
                    result = new Result(newQuality, bestFragment.Treil);
                }

                Options.NumberOfIterations--;
            }
            Log.Debug($"Best result: {result.Quality}");

            return result;
        }
    }
}
