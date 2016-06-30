using System;
using System.Linq;
using AlgorithmsCore.Contracts;
using AlgorithmsCore.Options;

namespace AlgorithmsCore
{
    public class WeightedAntSystemFragment : BaseAntSystemFragment
    {
        public int[] EdgesWeightOfColonies { get; }

        public WeightedAntSystemFragment(Random rnd, BaseOptions options, IGraph graph)
            : base(rnd, options, graph)
        {
            EdgesWeightOfColonies = new int[_options.NumberOfRegions];

            for (var i = 0; i < _options.NumberOfRegions; i++)
            {
                var randomFreeVertix = FreeVertices.Shuffle(rnd).First();

                AddFreeVertexToTreil(i, randomFreeVertix);
            }
        }

        protected override double GetSumOfOptimalityCriterion()
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
                Log.DebugFormat($"Region [{i}] | op: [{optimalityCriterions[i]}] | edge: [{EdgesWeightOfColonies[i]}] | col: [{WeightOfColonies[i]}]");
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

        public override sealed void AddFreeVertexToTreil(int colonyIndex, Vertex vertix)
        {
            FreeVertices.Remove(vertix);

            Treil[colonyIndex].Add(vertix);
            Log.DebugFormat($"Vertex: index: {vertix.Index}, weight: {vertix.Weight}");

            var currentWeightOfColony = WeightOfColonies[colonyIndex];
            WeightOfColonies[colonyIndex] = currentWeightOfColony + vertix.Weight;

            foreach (var passedVertex in Treil[colonyIndex])
            {
                EdgesWeightOfColonies[colonyIndex] += _graph.EdgesWeights[passedVertex.Index, vertix.Index];
            }
            
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
