using System;
using System.Collections.Generic;
using AlgorithmsCore;
using AlgorithmsCore.Contracts;
using AlgorithmsCore.Options;

namespace Basic
{
    public class AntSystemBasic : IAntSystem
    {
        private readonly Options _options;
        private readonly IGraph _graph;

        //TODO: AntSystemFragment DI here!!!!!
        private AntSystemFragment _antSystemFragment;
        //TODO: AntSystemFragment DI here!!!!!

        public AntSystemBasic(Random rnd, Options options, IGraph graph) 
        {
            _graph = graph;
            _options = options;

            _antSystemFragment = new AntSystemFragment(rnd, _options, _graph);
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
            var nextColonyId = _antSystemFragment.GetNextColony();
            return nextColonyId;
        }

        public double[] CalculateProbability(int nextColony)
        {
            var probability = _antSystemFragment.CalculateProbability(nextColony);
            return probability;
        }

        public AntSystemFragment UpdatePhermone(double maxAllowedWeight)
        {
            var sumOfOptimalityCriterions = _antSystemFragment.GetSumOfOptimalityCriterion(maxAllowedWeight);
            _graph.UpdatePhermone(_antSystemFragment.WeightOfColonies, _antSystemFragment.Treil, _options, sumOfOptimalityCriterions);
            return _antSystemFragment;
        }
    }
}
