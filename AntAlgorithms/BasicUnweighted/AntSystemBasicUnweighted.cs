using AlgorithmsCore;
using AlgorithmsCore.Contracts;
using AlgorithmsCore.Options;

namespace BasicUnweighted
{
    public class AntSystemBasicUnweighted : IAntSystem
    {
        private readonly BaseOptions _options;
        private readonly IGraph _graph;
        private readonly BaseAntSystemFragment _antSystemFragment;

        public AntSystemBasicUnweighted(BaseAntSystemFragment antSystemFragment, BaseOptions options, IGraph graph) 
        {
            _graph = graph;
            _options = options;
            _antSystemFragment = antSystemFragment;
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

        public decimal[] CalculateProbability(int nextColony)
        {
            var probability = _antSystemFragment.CalculateProbability(nextColony);
            return probability;
        }

        public BaseAntSystemFragment UpdatePhermone()
        {
            var sumOfOptimalityCriterions = _antSystemFragment.SumOfOptimalityCriterion;
            _graph.UpdatePhermone(_antSystemFragment.ColoniesConnections, _antSystemFragment.Treil, _options, sumOfOptimalityCriterions);
            return _antSystemFragment;
        }
    }
}
