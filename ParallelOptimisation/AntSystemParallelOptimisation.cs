﻿using System;
using System.Collections.Generic;
using System.Linq;
using AlgorithmsCore;
using AlgorithmsCore.Options;

namespace ParallelOptimisation
{
    public class AntSystemParallelOptimisation : AntSystemBase
    {
        private AntSystemFragment[] AntSystemFragments { get; }

        public AntSystemParallelOptimisation(Random rnd, OptionsParallelOptimisation options, IGraph graph)
            : base(rnd, options, graph)
        {
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

            var probability = CalculateProbability(antSystemFragment, nextColony);
            return probability;
        }

        public void AddFreeVertexToTreil(short interSectionId, int indexOfColony, Vertex vertix)
        {
            var antSystemFragment = AntSystemFragments[interSectionId];
            antSystemFragment.AddFreeVertexToTreil(indexOfColony, vertix);
        }

        public AntSystemFragment UpdatePhermone(double maxAllowedWeight)
        {
            var fragmentsOptimalityCriterion = new double[((OptionsParallelOptimisation) Options).NumberOfInterSections];
            for (int fragmentIndex = 0;
                fragmentIndex < ((OptionsParallelOptimisation) Options).NumberOfInterSections;
                fragmentIndex++)
            {
                fragmentsOptimalityCriterion[fragmentIndex] = AntSystemFragments[fragmentIndex].GetSumOfOptimalityCriterion(maxAllowedWeight);
            }

            double fragmentBestOptimalityCriterion = fragmentsOptimalityCriterion.Max();
            var indexOfFragmentWithBestQuality = Array.IndexOf(fragmentsOptimalityCriterion, fragmentBestOptimalityCriterion);

            var bestFragment = AntSystemFragments[indexOfFragmentWithBestQuality];

            UpdatePhermone(bestFragment, fragmentBestOptimalityCriterion);

            return bestFragment;
        }
    }
}
