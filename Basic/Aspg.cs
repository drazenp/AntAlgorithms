﻿using System;
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
            //int maxNumberOfVerticesInTrail = _graph.NumberOfVertices - _options.NumberOfRegions + 1;

            while (_options.NumberOfIterations > 0)
            {
                var antSystem = new AntSystem(_rnd, _options.NumberOfRegions, _graph);

                // prati broj cvorova u regionima
                int[,] korak = new int[_options.NumberOfRegions, 1];
                //ASPGOpcije.Korak = zeros(ASPGOpcije.h, 1); % prati broj cvorova u regionima

                for (int vertexIndex = _options.NumberOfRegions - 1; vertexIndex < _graph.NumberOfVertices; vertexIndex++)
                {
                    var nextColony = antSystem.GetNextColony();
                    double[] probability = CalculateProbability(antSystem, nextColony);

                }

                _options.NumberOfIterations--;
            }

            return quality;
        }

        public decimal GetMaxAllowedWeight(int[] verticesWeights)
        {
            var sumOfVerticesWeightes = verticesWeights.Sum();
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
                    pheromone += _graph.PheromoneMatrix[passedVertex, i];
                    edges += _graph.EdgesWeights[passedVertex, i];
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
    }
}
