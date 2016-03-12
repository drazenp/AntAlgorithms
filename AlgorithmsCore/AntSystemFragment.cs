using System;
using System.Collections.Generic;
using System.Linq;
using AlgorithmsCore.Contracts;
using log4net;

namespace AlgorithmsCore
{
    public class AntSystemFragment
    {
        static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            _log.DebugFormat($"Vertex i: {vertix.Index}, w: {vertix.Weight}");

            var currentWeightOfColony = WeightOfColonies[colonyIndex];
            WeightOfColonies[colonyIndex] = currentWeightOfColony + vertix.Weight;

            for (var i = 0; i < Treil[colonyIndex].Count; i++)
            {
                EdgesWeightOfColonies[colonyIndex] += _graph.EdgesWeights[i, vertix.Index];
            }

            PassedVertices.Add(vertix);
        }

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

        public double[] CalculateProbability(int nextColony)
        {
            double[] probability = new double[_graph.NumberOfVertices];

            var numberOfPassedVertices = Treil[nextColony].Count;

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
                    probability[freeVertex.Index] = Math.Pow(pheromone, _options.Alfa);
                }
                else
                {
                    probability[freeVertex.Index] = Math.Pow(pheromone, _options.Alfa) + Math.Pow(edges, _options.Beta);
                }
            }

            // In case probabilitySum is 0 is replaced with 1 since it's not possible to devide by zero.
            //   The results will be the same.
            // TODO: Check if probabilitySum can be replaced by constant.
            var probabilitySum = probability.Sum();
            if (Math.Abs(probabilitySum) < 0.0001)
            {
                probabilitySum = 1;
            }
            for (var i = 0; i < _graph.NumberOfVertices; i++)
            {
                probability[i] = probability[i] / probabilitySum;
            }

            return probability;
        }
    }
}
