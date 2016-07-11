using AlgorithmsCore.Options;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AlgorithmsCore.Contracts
{
    public abstract class BaseAntSystemFragment
    {
        protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected readonly BaseOptions _options;

        protected readonly IGraph _graph;

        protected readonly Random _rnd;

        public HashSet<Vertex> FreeVertices { get; }

        public HashSet<Vertex> PassedVertices { get; }  

        private double? _sumOfOptimalityCriterion;
        public double SumOfOptimalityCriterion
        {
            get
            {
                if (_sumOfOptimalityCriterion == null)
                {
                    _sumOfOptimalityCriterion = GetSumOfOptimalityCriterion();
                }
                return _sumOfOptimalityCriterion.Value;
            }
        }

        /// <summary>
        /// The sum of all vrtices weights for each colony.
        /// TODO: Check if should be used double insted int.
        /// </summary>
        public int[] WeightOfColonies { get; }

        public int[] ColoniesConnections { get; }

        /// <summary>
        /// For each colony define it's trail.
        /// </summary>
        public List<HashSet<Vertex>> Treil { get; /*Only for tests!!!!!*/protected set; }

        // TODO: Try to remove rnd and options from global variables; graph must remain at least for now.
        public BaseAntSystemFragment(Random rnd, BaseOptions options, IGraph graph)
        {
            _options = options;
            _graph = graph;
            _rnd = rnd;

            Treil = new List<HashSet<Vertex>>();
            for (var i = 0; i < _options.NumberOfRegions; i++)
            {
                Treil.Add(new HashSet<Vertex>());
            }

            FreeVertices = new HashSet<Vertex>(_graph.VerticesWeights);

            PassedVertices = new HashSet<Vertex>();

            WeightOfColonies = new int[_options.NumberOfRegions];

            ColoniesConnections = new int[_options.NumberOfRegions];
        }

        protected abstract double GetSumOfOptimalityCriterion();

        public abstract void AddFreeVertexToTreil(int colonyIndex, Vertex vertix);

        public abstract decimal[] CalculateProbability(int nextColony);

        /// <summary>
        /// Based on overall weight of colony shoose the next one.
        /// The next colony will be with the lowest weight.
        /// </summary>
        /// <returns>The ID of the next colony.</returns>
        public int GetNextColony()
        {
            var colonyWithMinWeight = WeightOfColonies.Min();
            return Array.IndexOf(WeightOfColonies, colonyWithMinWeight);
        }
    }
}
