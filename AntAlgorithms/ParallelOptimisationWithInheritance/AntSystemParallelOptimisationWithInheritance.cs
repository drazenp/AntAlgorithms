﻿using System;
using System.Linq;
using AlgorithmsCore;
using AlgorithmsCore.Contracts;
using AlgorithmsCore.Options;

namespace ParallelOptimisationWithInheritance
{
    public class AntSystemParallelOptimisationWithInheritance : IAntSystemParallelOptimisation
    {
        private Random _rnd;
        private readonly BaseOptions _options;
        private readonly IGraph _graph;

        private WeightedAntSystemFragment[] AntSystemFragments { get; }

        public AntSystemParallelOptimisationWithInheritance(Random rnd, OptionsParallelOptimisation options, IGraph graph, WeightedAntSystemFragment previousBestFragment)
        {
            _rnd = rnd;
            _graph = graph;
            _options = options;

            AntSystemFragments = new WeightedAntSystemFragment[options.NumberOfInterSections];

            short index = 0;
            if (previousBestFragment != null)
            {
                AntSystemFragments[0] = previousBestFragment;
                index = 1;
            }

            for (; index < options.NumberOfInterSections; index++)
            {
                AntSystemFragments[index] = new WeightedAntSystemFragment(rnd, options, _graph);
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

        public decimal[] CalculateProbability(short interSectionId, int nextColony)
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

        public WeightedAntSystemFragment UpdatePhermone()
        {
            var fragmentsOptimalityCriterion = new double[((OptionsParallelOptimisation)_options).NumberOfInterSections];
            for (var fragmentIndex = 0;
                fragmentIndex < ((OptionsParallelOptimisation)_options).NumberOfInterSections;
                fragmentIndex++)
            {
                fragmentsOptimalityCriterion[fragmentIndex] = AntSystemFragments[fragmentIndex].SumOfOptimalityCriterion;
            }

            double fragmentBestOptimalityCriterion = fragmentsOptimalityCriterion.Max();
            var indexOfFragmentWithBestQuality = Array.IndexOf(fragmentsOptimalityCriterion, fragmentBestOptimalityCriterion);

            var bestFragment = AntSystemFragments[indexOfFragmentWithBestQuality];

            _graph.UpdatePhermone(bestFragment.WeightOfColonies, bestFragment.Treil, _options, fragmentBestOptimalityCriterion);

            return bestFragment;
        }
    }
}
