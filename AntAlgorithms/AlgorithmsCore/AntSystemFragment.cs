using System;
using System.Collections.Generic;
using System.Linq;
using AlgorithmsCore.Contracts;
using log4net;

namespace AlgorithmsCore
{
    public class AntSystemFragment
    {
        static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Options.Options _options;
        private readonly IGraph _graph;

        /// <summary>
        /// For each colony define it's trail.
        /// </summary>
        public List<HashSet<Vertex>> Treil { get; }

        public HashSet<Vertex> FreeVertices { get; }

        public HashSet<Vertex> PassedVertices { get; }

        /// <summary>
        /// The sum of all vrtices weights for each colony.
        /// TODO: Check if should be used double insted int.
        /// </summary>
        public int[] WeightOfColonies { get; }

        public int[] EdgesWeightOfColonies { get; }

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

        // TODO: Try to remove rnd and options from global variables; graph must remain at least for now.
        public AntSystemFragment(Random rnd, Options.Options options, IGraph graph)
        {
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
                var randomFreeVertix = FreeVertices.Shuffle(rnd).First();

                AddFreeVertexToTreil(i, randomFreeVertix);
            }
        }

        public void AddFreeVertexToTreil(int colonyIndex, Vertex vertix)
        {
            FreeVertices.Remove(vertix);

            Treil[colonyIndex].Add(vertix);
            Log.DebugFormat($"Vertex i: {vertix.Index}, w: {vertix.Weight}");

            var currentWeightOfColony = WeightOfColonies[colonyIndex];
            WeightOfColonies[colonyIndex] = currentWeightOfColony + vertix.Weight;

            foreach (var passedVertex in Treil[colonyIndex])
            {
                EdgesWeightOfColonies[colonyIndex] += _graph.EdgesWeights[passedVertex.Index, vertix.Index];
            }
            
            PassedVertices.Add(vertix);
        }

        private double GetSumOfOptimalityCriterion()
        {
            double maxAllowedWeight = GetMaxAllowedWeight();
            var optimalityCriterions = new double[_options.NumberOfRegions];
            for (var i = 0; i < _options.NumberOfRegions; i++)
            {
                if (WeightOfColonies[i] < maxAllowedWeight)
                {
                    optimalityCriterions[i] = EdgesWeightOfColonies[i];
                }
                else
                {
                    optimalityCriterions[i] = EdgesWeightOfColonies[i] - 1000 * (WeightOfColonies[i] - maxAllowedWeight);
                }
                Log.DebugFormat($"Region [{i}] | op: [{optimalityCriterions[i]}] | edgw: [{EdgesWeightOfColonies[i]}] | colw: [{WeightOfColonies[i]}]");
            }
            
            var sumOfOptimalityCriterions = optimalityCriterions.Sum();
            Log.DebugFormat($"sumOfOptimalityCriterions: {sumOfOptimalityCriterions}");
            return sumOfOptimalityCriterions;
        }

        private double GetMaxAllowedWeight()
        {
            var sumOfVerticesWeightes = _graph.VerticesWeights.Select(v => v.Weight).Sum();
            double maxAllowedWeight = sumOfVerticesWeightes / (double)_options.NumberOfRegions * (1 + _options.Delta);
            return maxAllowedWeight;
        }

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

        public decimal[] CalculateProbability(int nextColony)
        {
            decimal[] probability = new decimal[_graph.NumberOfVertices];

            var numberOfPassedVertices = Treil[nextColony].Count;

            //Utility.LogDoubleMatrixAsTable(_graph.PheromoneMatrix);

            foreach (var freeVertex in FreeVertices)
            {
                var pheromone = 0D;
                var edges = 0;
                foreach (var passedVertex in Treil[nextColony])
                {
                    pheromone += _graph.PheromoneMatrix[passedVertex.Index, freeVertex.Index];
                    edges += _graph.EdgesWeights[passedVertex.Index, freeVertex.Index];
                }
                pheromone /= numberOfPassedVertices;

                if (edges == 0)
                {
                    probability[freeVertex.Index] = (decimal)Math.Pow(pheromone, _options.Alfa);
                }
                else
                {
                    probability[freeVertex.Index] = (decimal)Math.Pow(pheromone, _options.Alfa) * (decimal)Math.Pow(edges, _options.Beta);
                }
            }

            // In case probabilitySum is 0 is replaced with 1 since it's not possible to devide by zero.
            //   The results will be the same.
            // TODO: Check if probabilitySum can be replaced by constant.
            var probabilitySum = probability.Sum();
            if (Math.Abs(probabilitySum) == 0M)
            {
                probabilitySum = 1;
            }
            for (var i = 0; i < _graph.NumberOfVertices; i++)
            {
                probability[i] = probability[i] / probabilitySum;
            }

            //Utility.LogDecimalArrayAsList(probability);
            return probability;
        }
    }
}
