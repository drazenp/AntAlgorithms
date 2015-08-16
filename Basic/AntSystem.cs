using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// The sum of all vrtices weights for each colony.
        /// </summary>
        public decimal[] SumOfRegionWeight { get; private set; }

        public HashSet<Vertex> FreeVertices { get; private set; }

        public HashSet<Vertex> PassedVertices { get; private set; }

        // Overall weight of the all colonies.
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

            SumOfRegionWeight = new decimal[numberOfRegions];

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

        public decimal[] CalculateOptimalityCriterion(decimal maxAllowedWeight)
        {
            var optimalityCriterions = new decimal[_numberOfRegions];
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
    }
}
