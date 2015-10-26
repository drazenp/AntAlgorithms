using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsCore
{
    public class AntSystemBase
    {
        protected readonly Options.Options Options;
        protected readonly IGraph Graph;

        public AntSystemBase(Random rnd, Options.Options options, IGraph graph)
        {
            Options = options;
            Graph = graph;
        }

        // TODO: The function must be refactored. It should not return sumOfOptimalityCriterions.
        // TODO: It must be checked if can be implemenet pheromone update in graph class.
        public void UpdatePhermone(AntSystemFragment antSystemFragment, double sumOfOptimalityCriterions)
        {
            // Colony with heighes weight.
            var colonyWithHighestWeight = Array.IndexOf(antSystemFragment.WeightOfColonies, antSystemFragment.WeightOfColonies.Max());

            // If criterions of optimality is less then 0, the minimum of pheromones will be set.
            if (sumOfOptimalityCriterions > 0)
            {
                for (var indexOfRegion = 0; indexOfRegion < Options.NumberOfRegions; indexOfRegion++)
                {
                    double pheromoneToSet;
                    if (indexOfRegion == colonyWithHighestWeight)
                    {
                        pheromoneToSet = 0.01D * (sumOfOptimalityCriterions + 1.2D * antSystemFragment.WeightOfColonies[colonyWithHighestWeight]);
                    }
                    else
                    {
                        pheromoneToSet = Constants.MinimalVelueOfPheromoneToSet;
                    }

                    var path = antSystemFragment.Treil[indexOfRegion];
                    foreach (var vertex1 in path)
                    {
                        foreach (var vertex2 in path.Skip(1))
                        {
                            Graph.PheromoneMatrix[vertex1.Index, vertex2.Index] = Graph.PheromoneMatrix[vertex1.Index, vertex2.Index] * (1 - Options.Ro) + pheromoneToSet;
                            Graph.PheromoneMatrix[vertex2.Index, vertex1.Index] = Graph.PheromoneMatrix[vertex2.Index, vertex1.Index] * (1 - Options.Ro) + pheromoneToSet;
                        }
                    }
                }
            }
            else
            {
                for (var i = 0; i < Graph.PheromoneMatrix.GetLength(0); i++)
                {
                    for (var j = 0; j < Graph.PheromoneMatrix.GetLength(1); j++)
                    {
                        Graph.PheromoneMatrix[i, j] = Graph.PheromoneMatrix[i, j] * (1 - Options.Ro) + Constants.MinimalVelueOfPheromoneToSet;
                    }
                }
            }
        }
    }
}
