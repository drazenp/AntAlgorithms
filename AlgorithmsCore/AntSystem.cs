﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsCore
{
    public class AntSystem
    {
        private readonly Random _rnd;
        private readonly Options _options;
        private readonly IGraph _graph;

        /// <summary>
        /// For each colony define it's trail.
        /// </summary>
        public List<HashSet<Vertex>> Treil { get; }

        public HashSet<Vertex> FreeVertices { get; }

        public HashSet<Vertex> PassedVertices { get;  }

        /// <summary>
        /// The sum of all vrtices weights for each colony.
        /// TODO: Check if should be used decimal insted int.
        /// </summary>
        public int[] WeightOfColonies { get; }

        public int[] EdgesWeightOfColonies { get; }

        public AntSystem(Random rnd, Options options, IGraph graph)
        {
            _rnd = rnd;
            _options = options;
            _graph = graph;

            Treil = new List<HashSet<Vertex>>();
            for (var i = 0; i < _options.NumberOfRegions; i++)
            {
                Treil.Add(new HashSet<Vertex>());
            }
            
            FreeVertices = new HashSet<Vertex>(graph.VerticesWeights);

            PassedVertices = new HashSet<Vertex>();

            WeightOfColonies = new int[_options.NumberOfRegions];

            EdgesWeightOfColonies = new int[_options.NumberOfRegions];
        }

        public void InitializeTreils()
        {
            for (var i = 0; i < _options.NumberOfRegions; i++)
            {
                var randomFreeVertix = FreeVertices.Shuffle(_rnd).First();

                AddFreeVertexToTreil(i, randomFreeVertix);
            }
        }

        public void AddFreeVertexToTreil(int colonyIndex, Vertex vertix)
        {
            FreeVertices.Remove(vertix);

            Treil[colonyIndex].Add(vertix);

            var currentWeightOfColony = WeightOfColonies[colonyIndex];
            WeightOfColonies[colonyIndex] = currentWeightOfColony + vertix.Weight;

            for (var i = 0; i < Treil[colonyIndex].Count; i++)
            {
                EdgesWeightOfColonies[colonyIndex] += _graph.EdgesWeights[i, vertix.Index];
            }

            PassedVertices.Add(vertix);
        }

        /// <summary>
        /// Based on overall weight of colony shoose the next one.
        /// The next colony will be with the lowest weight.
        /// </summary>
        /// <returns></returns>
        public int GetNextColony()
        {
            var colonyWithMinWeight = WeightOfColonies.Min();
            return Array.IndexOf(WeightOfColonies, colonyWithMinWeight);
        }

        public double[] CalculateProbability(int nextColony)
        {
            double[] probability = new double[_graph.NumberOfVertices];

            var numberOfPassedVertices = Treil[nextColony].Count;

            foreach (var freeVertex in FreeVertices)
            {
                var pheromone = 0D;
                var edges = 0;
                foreach (var passedVertex in Treil[nextColony])
                {
                    pheromone += _graph.PheromoneMatrix[passedVertex.Index, freeVertex.Index];
                    edges += _graph.EdgesWeights[passedVertex.Index, freeVertex.Index];
                }
                pheromone /= numberOfPassedVertices;

                if (edges == 0)
                {
                    probability[freeVertex.Index] = Math.Pow(pheromone, _options.Alfa);
                }
                else
                {
                    probability[freeVertex.Index] = Math.Pow(pheromone, _options.Alfa) + Math.Pow(edges, _options.Beta);
                }
            }

            var probabilitySum = probability.Sum();
            for (int i = 0; i < _graph.NumberOfVertices; i++)
            {
                probability[i] = probability[i] / probabilitySum;
            }

            return probability;
        }

        public double[] CalculateOptimalityCriterion(double maxAllowedWeight)
        {
            var optimalityCriterions = new double[_options.NumberOfRegions];
            for (var i = 0; i < _options.NumberOfRegions; i++)
            {
                optimalityCriterions[i] = Math.Abs(EdgesWeightOfColonies[i] - 1000 * (WeightOfColonies[i] - maxAllowedWeight));
            }
            return optimalityCriterions;
        }

        // TODO: The function must be refactored. It should not return sumOfOptimalityCriterions;
        // ..._options.ro is missing; it must be checked if can be implemenet pheromone update in graph class.
        public double UpdatePhermone(double maxAllowedWeight)
        {
            var optimalityCriterions = CalculateOptimalityCriterion(maxAllowedWeight);
            var sumOfOptimalityCriterions = optimalityCriterions.Sum();

            // Colony with heighes weight.
            var colonyWithHeigWeight = Array.IndexOf(WeightOfColonies, WeightOfColonies.Max());

            // If criterions of optimality is less then 0, the minimum of pheromones will be set.
            if (sumOfOptimalityCriterions > 0)
            {
                for (var indexOfRegion = 0; indexOfRegion < _options.NumberOfRegions; indexOfRegion++)
                {
                    double pheromoneToSet;
                    if (indexOfRegion == colonyWithHeigWeight)
                    {
                        pheromoneToSet = 0.01D * (sumOfOptimalityCriterions + 1.2D * WeightOfColonies[colonyWithHeigWeight]);
                    }
                    else
                    {
                        pheromoneToSet = Constants.MinimalVelueOfPheromoneToSet;
                    }

                    var path = Treil[indexOfRegion];
                    foreach (var vertex1 in path)
                    {
                        foreach (var vertex2 in path.Skip(1))
                        {
                            _graph.PheromoneMatrix[vertex1.Index, vertex2.Index] = _graph.PheromoneMatrix[vertex1.Index, vertex2.Index] * (1 - _options.Ro) + pheromoneToSet;
                            _graph.PheromoneMatrix[vertex2.Index, vertex1.Index] = _graph.PheromoneMatrix[vertex2.Index, vertex1.Index] * (1 - _options.Ro) + pheromoneToSet;
                        }
                    }
                }
            }
            else
            {
                for (var i = 0; i < _graph.PheromoneMatrix.GetLength(0); i++)
                {
                    for (var j = 0; j < _graph.PheromoneMatrix.GetLength(1); j++)
                    {
                        _graph.PheromoneMatrix[i, j] = _graph.PheromoneMatrix[i, j] * (1 - _options.Ro) + Constants.MinimalVelueOfPheromoneToSet;
                    }
                }
            }

            return sumOfOptimalityCriterions;
        }
    }
}
