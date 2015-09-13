using System;
using System.Collections.Generic;
using AlgorithmsCore;
using AlgorithmsCore.Options;

namespace Basic
{
    public class AntSystemBasic : AntSystemBase
    {
        //TODO: AntSystemFragment DI here!!!!!
        private AntSystemFragment _antSystemFragment;
        //TODO: AntSystemFragment DI here!!!!!

        public AntSystemBasic(Random rnd, Options options, IGraph graph) 
            : base(rnd, options, graph)
        {
            _antSystemFragment = new AntSystemFragment(rnd, options, graph);
        }

        public void AddFreeVertexToTreil(int indexOfColony, Vertex vertix)
        {
            _antSystemFragment.AddFreeVertexToTreil(indexOfColony, vertix);
        }
        /// <summary>
        /// Based on overall weight of colony shoose the next one.
        /// The next colony will be with the lowest weight.
        /// </summary>
        /// <returns></returns>
        public int GetNextColony()
        {
            var nextColonyId = GetNextColony(_antSystemFragment);
            return nextColonyId;
        }

        public double[] CalculateProbability(int nextColony)
        {
            var probability = CalculateProbability(_antSystemFragment, nextColony);
            return probability;
        }

        public double UpdatePhermone(double maxAllowedWeight)
        {
            var sumOfOptimalityCriterions = _antSystemFragment.GetSumOfOptimalityCriterion(maxAllowedWeight);
            UpdatePhermone(_antSystemFragment, sumOfOptimalityCriterions);
            return sumOfOptimalityCriterions;
        }

        public List<HashSet<Vertex>> GetTrails()
        {
            var trails = _antSystemFragment.Treil;
            return trails;
        }
    }
}
