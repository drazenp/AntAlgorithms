using System;
using System.Collections.Generic;
using System.Linq;
using AlgorithmsCore;
using AlgorithmsCore.Options;

namespace ParallelOptimisation
{
    public class AntSystemParallelOptimisation
    {
        private Random _rnd;
        private readonly Options _options;
        private readonly IGraph _graph;

        private AntSystemFragment[] AntSystemFragments { get; }

        public AntSystemParallelOptimisation(Random rnd, OptionsParallelOptimisation options, IGraph graph)
        {
            _rnd = rnd;
            _graph = graph;
            _options = options;

            AntSystemFragments = new AntSystemFragment[options.NumberOfInterSections];
            for (var i = 0; i < options.NumberOfInterSections; i++)
            {
                AntSystemFragments[i] = new AntSystemFragment(rnd, options, graph);
            }
        }

        /// <summary>
        /// Based on overall weight of colony shoose the next one.
        /// The next colony will be with the lowest weight.
        /// </summary>
        /// <param name="interSectionId">The ID of the inter section.</param>
        /// <returns>The ID of the next colony of the inter section.</returns>
        public int GetNextColony(short interSectionId)
        {
            var antSystemFragment = AntSystemFragments[interSectionId];
            var nextColonyId = antSystemFragment.GetNextColony();
            return nextColonyId;
        }

        public double[] CalculateProbability(short interSectionId, int nextColony)
        {
            var antSystemFragment = AntSystemFragments[interSectionId];

            var probability = antSystemFragment.CalculateProbability(nextColony);
            return probability;
        }

        public void AddFreeVertexToTreil(short interSectionId, int indexOfColony, Vertex vertix)
        {
            var antSystemFragment = AntSystemFragments[interSectionId];
            antSystemFragment.AddFreeVertexToTreil(indexOfColony, vertix);
        }

        /// <summary>
        /// Update pheromone on graph based on quality.
        /// </summary>
        /// <param name="maxAllowedWeight"></param>
        /// <returns>The fragment with best quality.</returns>
        public AntSystemFragment UpdatePhermone(double maxAllowedWeight)
        {
            var fragmentsOptimalityCriterion = new double[((OptionsParallelOptimisation)_options).NumberOfInterSections];
            for (int fragmentIndex = 0;
                fragmentIndex < ((OptionsParallelOptimisation)_options).NumberOfInterSections;
                fragmentIndex++)
            {
                fragmentsOptimalityCriterion[fragmentIndex] = AntSystemFragments[fragmentIndex].GetSumOfOptimalityCriterion(maxAllowedWeight);
            }

            double fragmentBestOptimalityCriterion = fragmentsOptimalityCriterion.Max();
            var indexOfFragmentWithBestQuality = Array.IndexOf(fragmentsOptimalityCriterion, fragmentBestOptimalityCriterion);

            var bestFragment = AntSystemFragments[indexOfFragmentWithBestQuality];

            _graph.UpdatePhermone(bestFragment, _options, fragmentBestOptimalityCriterion);

            return bestFragment;
        }
    }
}
