﻿using System;
using System.Linq;
using log4net;

namespace AlgorithmsCore.Contracts
{
    public abstract class AspgBase
    {
        protected static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected readonly Options.Options Options;
        protected readonly IGraph Graph;
        protected readonly Random Rnd;

        protected AspgBase(Options.Options options, IGraph graph, Random rnd)
        {
            Options = options;
            Graph = graph;
            Rnd = rnd;
        }

        public abstract Result GetQuality();

        public double GetMaxAllowedWeight()
        {
            var sumOfVerticesWeightes = Graph.VerticesWeights.Select(v => v.Weight).Sum();
            double maxAllowedWeight = sumOfVerticesWeightes / (double)Options.NumberOfRegions * (1 + Options.Delta);
            return maxAllowedWeight;
        }

        public int Roulette(double[] probability)
        {
            var boundary = Rnd.NextDouble();
            var currentSumOfProbability = 0D;
            for (int i = 0; i < Graph.NumberOfVertices; i++)
            {
                currentSumOfProbability += probability[i];
                if (boundary <= currentSumOfProbability)
                {
                    return i;
                }
            }

            return Graph.NumberOfVertices - 1;
        }
    }
}
