using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsCore
{
    public class AntSystemBase
    {
        protected readonly Options.Options Options;
        protected readonly IGraph Graph;

        protected AntSystemBase(Random rnd, Options.Options options, IGraph graph)
        {
            Options = options;
            Graph = graph;
        }
        
        /// <summary>
        /// Based on overall weight of colony shoose the next one.
        /// The next colony will be with the lowest weight.
        /// </summary>
        /// <returns></returns>
        protected int GetNextColony(AntSystemFragment antSystemFragment)
        {
            var colonyWithMinWeight = antSystemFragment.WeightOfColonies.Min();
            return Array.IndexOf(antSystemFragment.WeightOfColonies, colonyWithMinWeight);
        }

        protected double[] CalculateProbability(AntSystemFragment antSystemFragment, int nextColony)
        {
            double[] probability = new double[Graph.NumberOfVertices];

            var numberOfPassedVertices = antSystemFragment.Treil[nextColony].Count;

            foreach (var freeVertex in antSystemFragment.FreeVertices)
            {
                var pheromone = 0D;
                var edges = 0;
                foreach (var passedVertex in antSystemFragment.Treil[nextColony])
                {
                    pheromone += Graph.PheromoneMatrix[passedVertex.Index, freeVertex.Index];
                    edges += Graph.EdgesWeights[passedVertex.Index, freeVertex.Index];
                }
                pheromone /= numberOfPassedVertices;

                if (edges == 0)
                {
                    probability[freeVertex.Index] = Math.Pow(pheromone, Options.Alfa);
                }
                else
                {
                    probability[freeVertex.Index] = Math.Pow(pheromone, Options.Alfa) + Math.Pow(edges, Options.Beta);
                }
            }

            var probabilitySum = probability.Sum();
            for (var i = 0; i < Graph.NumberOfVertices; i++)
            {
                probability[i] = probability[i] / probabilitySum;
            }

            return probability;
        }

        // TODO: The function must be refactored. It should not return sumOfOptimalityCriterions.
        // TODO: It must be checked if can be implemenet pheromone update in graph class.
        protected void UpdatePhermone(AntSystemFragment antSystemFragment, double sumOfOptimalityCriterions)
        {
            // Colony with heighes weight.
            var colonyWithHeigWeight = Array.IndexOf(antSystemFragment.WeightOfColonies, antSystemFragment.WeightOfColonies.Max());

            // If criterions of optimality is less then 0, the minimum of pheromones will be set.
            if (sumOfOptimalityCriterions > 0)
            {
                for (var indexOfRegion = 0; indexOfRegion < Options.NumberOfRegions; indexOfRegion++)
                {
                    double pheromoneToSet;
                    if (indexOfRegion == colonyWithHeigWeight)
                    {
                        pheromoneToSet = 0.01D * (sumOfOptimalityCriterions + 1.2D * antSystemFragment.WeightOfColonies[colonyWithHeigWeight]);
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
