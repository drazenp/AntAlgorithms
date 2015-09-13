using System;
using AlgorithmsCore;
using AlgorithmsCore.Options;

namespace Basic
{
    public class Aspg : AspgBase
    {
        public Aspg(Options options, IGraph graph, Random rnd) 
            : base (options, graph, rnd) { }

        public override Result GetQuality()
        {
            var result = new Result(double.MinValue);
            var maxAllowedWeight = GetMaxAllowedWeight();

            while (Options.NumberOfIterations > 0)
            {
                var antSystem = new AntSystemBasic(Rnd, Options, Graph);

                for (var vertexIndex = Options.NumberOfRegions; vertexIndex < Graph.NumberOfVertices; vertexIndex++)
                {
                    var nextColony = antSystem.GetNextColony();
                    double[] probability = antSystem.CalculateProbability(nextColony);
                    var chosenVertexIndex = Roulette(probability);
                    var chosenVertex = Graph.VerticesWeights[chosenVertexIndex];
                    antSystem.AddFreeVertexToTreil(nextColony, chosenVertex);
                }

                var sumOfOptimalityCriterions = antSystem.UpdatePhermone(maxAllowedWeight);

                // Save the best results.
                if (result.Quality < sumOfOptimalityCriterions)
                {
                    result = new Result(sumOfOptimalityCriterions, antSystem.GetTrails());
                }

                Options.NumberOfIterations--;
            }

            return result;
        }
    }
}
