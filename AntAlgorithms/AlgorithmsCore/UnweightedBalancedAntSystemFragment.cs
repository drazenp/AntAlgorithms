using System;
using System.Linq;
using AlgorithmsCore.Contracts;
using AlgorithmsCore.Options;

namespace AlgorithmsCore
{
    public class UnweightedBalancedAntSystemFragment : BaseAntSystemFragment
    {
        public UnweightedBalancedAntSystemFragment(Random rnd, BaseOptions options, IGraph graph)
            : base(rnd, options, graph)
        {
            for (var i = 0; i < _options.NumberOfRegions; i++)
            {
                var randomFreeVertix = FreeVertices.Shuffle(rnd).First();

                AddFreeVertexToTreil(i, randomFreeVertix);
            }
        }

        protected override double GetSumOfOptimalityCriterion()
        {
            for (var i = 0; i < _options.NumberOfRegions; i++)
            {
                var path = Treil[i];
                var verexCombination = path.SelectMany((value, index) => path.Skip(index + 1),
                                                       (first, second) => new { first, second });

                foreach (var combination in verexCombination)
                {
                    var edgeWeight = _graph.EdgesWeights[combination.first.Index, combination.second.Index];
                    Log.DebugFormat($"[{i}] first<{combination.first.Index}> second<{combination.second.Index}> |{edgeWeight}|");
                    ColoniesConnections[i] += edgeWeight;
                }
            }

            var sumOfOptimalityCriterions = ColoniesConnections.Sum();
            Log.DebugFormat($"SumOfOptimalityCriterions: {sumOfOptimalityCriterions}");
            return sumOfOptimalityCriterions;
        }

        public override sealed void AddFreeVertexToTreil(int colonyIndex, Vertex vertix)
        {
            FreeVertices.Remove(vertix);

            Treil[colonyIndex].Add(vertix);
            WeightOfColonies[colonyIndex]++;

            Log.DebugFormat($"Vertex <{vertix.Index}> add for colony <{colonyIndex}>");

            PassedVertices.Add(vertix);
        }

        public override decimal[] CalculateProbability(int nextColony)
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
