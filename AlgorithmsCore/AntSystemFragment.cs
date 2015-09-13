using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsCore
{
    public class AntSystemFragment
    {
        private readonly Random _rnd;
        private readonly Options _options;
        private readonly IGraph _graph;

        /// <summary>
        /// For each colony define it's trail.
        /// </summary>
        public List<HashSet<Vertex>> Treil { get; }

        public HashSet<Vertex> FreeVertices { get; }

        public HashSet<Vertex> PassedVertices { get; }

        /// <summary>
        /// The sum of all vrtices weights for each colony.
        /// TODO: Check if should be used decimal insted int.
        /// </summary>
        public int[] WeightOfColonies { get; }

        public int[] EdgesWeightOfColonies { get; }

        // TODO: Try to remove rnd and options from global variables; graph must remain at least for now.
        public AntSystemFragment(Random rnd, Options options, IGraph graph)
        {
            _rnd = rnd;
            _options = options;
            _graph = graph;

            Treil = new List<HashSet<Vertex>>();
            for (var i = 0; i < _options.NumberOfRegions; i++)
            {
                Treil.Add(new HashSet<Vertex>());
            }

            FreeVertices = new HashSet<Vertex>(_graph.VerticesWeights);

            PassedVertices = new HashSet<Vertex>();

            WeightOfColonies = new int[_options.NumberOfRegions];

            EdgesWeightOfColonies = new int[_options.NumberOfRegions];

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

        //public double[] CalculateOptimalityCriterion(double maxAllowedWeight)
        //{
        //    var optimalityCriterions = new double[_options.NumberOfRegions];
        //    for (var i = 0; i < _options.NumberOfRegions; i++)
        //    {
        //        optimalityCriterions[i] = Math.Abs(EdgesWeightOfColonies[i] - 1000 * (WeightOfColonies[i] - maxAllowedWeight));
        //    }
        //    return optimalityCriterions;
        //}

        public double GetSumOfOptimalityCriterion(double maxAllowedWeight)
        {
            var optimalityCriterions = new double[_options.NumberOfRegions];
            for (var i = 0; i < _options.NumberOfRegions; i++)
            {
                optimalityCriterions[i] = Math.Abs(EdgesWeightOfColonies[i] - 1000 * (WeightOfColonies[i] - maxAllowedWeight));
            }
             
            var sumOfOptimalityCriterions = optimalityCriterions.Sum();
            return sumOfOptimalityCriterions;
        }
    }
}
