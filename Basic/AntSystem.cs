using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Basic
{
    public class AntSystem
    {
        private readonly Random _rnd;
        private readonly int _numberOfRegions;
        private readonly IGraph _graph;

        /// <summary>
        /// For each colony define it's trail.
        /// </summary>
        public List<HashSet<Vertex>> Treil { get; private set; }

        public HashSet<Vertex> FreeVertices { get; private set; }

        public HashSet<Vertex> PassedVertices { get; private set; }

        /// <summary>
        /// The sum of all vrtices weights for each colony.
        /// TODO: Check if should be used decimal insted int.
        /// </summary>
        public int[] WeightOfColonies { get; private set; }

        public int[] EdgesWeightOfColonies { get; private set; }

        public AntSystem(Random rnd, int numberOfRegions, IGraph graph)
        {
            _rnd = rnd;
            _numberOfRegions = numberOfRegions;
            _graph = graph;

            Treil = new List<HashSet<Vertex>>();
            for (var i = 0; i < numberOfRegions; i++)
            {
                Treil.Add(new HashSet<Vertex>());
            }

            FreeVertices = new HashSet<Vertex>(graph.VerticesWeights);

            PassedVertices = new HashSet<Vertex>();

            WeightOfColonies = new int[numberOfRegions];

            EdgesWeightOfColonies = new int[numberOfRegions];
        }

        public void InitializeTreils()
        {
            for (var i = 0; i < _numberOfRegions; i++)
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

            for (int i = 0; i < Treil[colonyIndex].Count; i++)
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

        public double[] CalculateOptimalityCriterion(double maxAllowedWeight)
        {
            var optimalityCriterions = new double[_numberOfRegions];
            for (int i = 0; i < _numberOfRegions; i++)
            {
                optimalityCriterions[i] = EdgesWeightOfColonies[i] - 1000 * (WeightOfColonies[i] - maxAllowedWeight);
                if (WeightOfColonies[i] <= maxAllowedWeight)
                {
                    optimalityCriterions[i] *= -1;
                }
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
                for (var indexOfRegion = 0; indexOfRegion < _numberOfRegions; indexOfRegion++)
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
                            // TODO: 0.1 must be replaced with _options.ro.
                            _graph.PheromoneMatrix[vertex1.Index, vertex2.Index] = _graph.PheromoneMatrix[vertex1.Index, vertex2.Index] * (1 - 0.1) + pheromoneToSet;
                            _graph.PheromoneMatrix[vertex2.Index, vertex1.Index] = _graph.PheromoneMatrix[vertex2.Index, vertex1.Index] * (1 - 0.1) + pheromoneToSet;
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
                        // TODO: 0.1 must be replaced with _options.ro.
                        _graph.PheromoneMatrix[i, j] = _graph.PheromoneMatrix[i, j] * (1 - 0.1) + Constants.MinimalVelueOfPheromoneToSet;
                    }
                }
            }

            return sumOfOptimalityCriterions;
            // 
            /*
            	% rh predstavlja faktor isparavanja, a sumdtau promena koja ce se obaviti nad tim poljem.
                % U slucaju da je taj potez deo kvalitetnog re{enja, vrednost ce se povecati 
                % za neku vrednost, dok ce u suprotnom izraz poprimiti vrednost 0
                Problem.tau=Problem.tau*(1-ASPGOpcije.ro)+sumdtau;
            */
        }
    }
}
