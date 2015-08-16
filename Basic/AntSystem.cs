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
        public List<HashSet<int>> Treil { get; private set; }

        /// <summary>
        /// The sum of all vrtices weights for each colony.
        /// </summary>
        public decimal[] SumOfRegionWeight { get; private set; }

        public HashSet<int> FreeVertices { get; private set; }

        public HashSet<int> PassedVertices { get; private set; }

        // Overall weight of the all colonies.
        public int[] WeightOfColonies { get; private set; }

        public int[] EdgesWeightOfColonies { get; private set; }

        public AntSystem(Random rnd, int numberOfRegions, IGraph graph)
        {
            _rnd = rnd;
            _numberOfRegions = numberOfRegions;
            _graph = graph;

            Treil = new List<HashSet<int>>();
            for (var i = 0; i < numberOfRegions; i++)
            {
                Treil.Add(new HashSet<int>());
            }

            SumOfRegionWeight = new decimal[numberOfRegions];

            FreeVertices = new HashSet<int>(graph.VerticesWeights);

            PassedVertices = new HashSet<int>();

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

        public void AddFreeVertexToTreil(int colonyIndex, int vertix)
        {
            FreeVertices.Remove(vertix);

            Treil[colonyIndex].Add(vertix);

            var currentWeightOfColony = WeightOfColonies[colonyIndex];
            WeightOfColonies[colonyIndex] = currentWeightOfColony + vertix;

            for (int i = 0; i < Treil[colonyIndex].Count; i++)
            {
                EdgesWeightOfColonies[colonyIndex] += _graph.EdgesWeights[i, vertix];
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
    }
}
