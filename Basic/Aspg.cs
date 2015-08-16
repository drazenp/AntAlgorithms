using System;
using System.Collections.Generic;
using System.Linq;

namespace Basic
{
    public class Aspg
    {
        private Options _options;
        private readonly IGraph _graph;

        private readonly Random _rnd = new Random(Environment.TickCount);

        public Aspg(Options options, IGraph graph)
        {
            _options = options;
            _graph = graph;
        }

        public decimal GetQuality()
        {
            decimal quality = decimal.MinValue;
            var maxAllowedWeight = GetMaxAllowedWeight();

            while (_options.NumberOfIterations > 0)
            {
                var antSystem = new AntSystem(_rnd, _options.NumberOfRegions, _graph);
                
                for (int vertexIndex = _options.NumberOfRegions - 1; vertexIndex < _graph.NumberOfVertices; vertexIndex++)
                {
                    var nextColony = antSystem.GetNextColony();
                    double[] probability = CalculateProbability(antSystem, nextColony);
                    var chosenVertexIndex = Roulette(probability);
                    var chosenVertex = _graph.VerticesWeights[chosenVertexIndex];
                    antSystem.AddFreeVertexToTreil(nextColony, chosenVertex);
                }

                //SumaKriterijumaOptimalnosti(dozvola, TC);

                _options.NumberOfIterations--;
            }

            return quality;
        }

        public decimal GetMaxAllowedWeight()
        {
            var sumOfVerticesWeightes = _graph.VerticesWeights.Select(v => v.Weight).Sum();
            decimal maxAllowedWeight = sumOfVerticesWeightes / (decimal)_options.NumberOfRegions * (1 + _options.Delta);
            return maxAllowedWeight;
        }

        public double[] CalculateProbability(AntSystem antSystem, int nextColony)
        {
            var numberOfFreeVertices = antSystem.FreeVertices.Count;
            double[] probability = new double[numberOfFreeVertices];

            var numberOfPassedVertices = antSystem.Treil[nextColony].Count;

            for (int i = 0; i < numberOfFreeVertices; i++)
            {
                var pheromone = 0D;
                var edges = 0;
                foreach (var passedVertex in antSystem.Treil[nextColony])
                {
                    pheromone += _graph.PheromoneMatrix[passedVertex.Index, i];
                    edges += _graph.EdgesWeights[passedVertex.Index, i];
                }
                pheromone /= numberOfPassedVertices;

                if (edges == 0)
                {
                    probability[i] = Math.Pow(pheromone, _options.Alfa);
                }
                else
                {
                    probability[i] = Math.Pow(pheromone, _options.Alfa) + Math.Pow(edges, _options.Beta);
                }
            }

            var probabilitySum = probability.Sum();
            for (int i = 0; i < numberOfFreeVertices; i++)
            {
                probability[i] = probability[i] / probabilitySum;
            }

            return probability;
        }

        public int Roulette(double[] probability)
        {
            var boundary = _rnd.NextDouble();
            var currentSumOfProbability = 0D;
            for (int i = 0; i < _graph.NumberOfVertices; i++)
            {
                currentSumOfProbability += probability[i];
                if (boundary <= currentSumOfProbability)
                {
                    return i;
                }
            }

            return _graph.NumberOfVertices - 1;
        }
    }
}
