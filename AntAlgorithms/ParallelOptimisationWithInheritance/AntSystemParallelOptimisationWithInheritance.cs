using System;
using System.Collections.Generic;
using System.Linq;
using AlgorithmsCore;
using AlgorithmsCore.Options;

namespace ParallelOptimisationWithInheritance
{
    public class AntSystemParallelOptimisationWithInheritance : AntSystemBase
    {
        private AntSystemFragment[] AntSystemFragments { get; }

        public List<HashSet<Vertex>> BestTreil { get; set; }

        public double FragmentBestOptimalityCriterion { get; set; }

        public AntSystemParallelOptimisationWithInheritance(Random rnd, OptionsParallelOptimisation options, IGraph graph, AntSystemFragment previousBestFragment)
            : base(rnd, options, graph)
        {
            AntSystemFragments = new AntSystemFragment[options.NumberOfInterSections];

            short index = 0;
            if (previousBestFragment != null)
            {
                AntSystemFragments[0] = previousBestFragment;
                index = 1;
            }

            for (; index < options.NumberOfInterSections; index++)
            {
                AntSystemFragments[index] = new AntSystemFragment(rnd, options, graph);
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
            var nextColonyId = GetNextColony(antSystemFragment);
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
            var fragmentsOptimalityCriterion = new double[((OptionsParallelOptimisation)Options).NumberOfInterSections];
            for (int fragmentIndex = 0;
                fragmentIndex < ((OptionsParallelOptimisation)Options).NumberOfInterSections;
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
