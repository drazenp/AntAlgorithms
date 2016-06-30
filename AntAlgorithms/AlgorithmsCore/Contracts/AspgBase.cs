using System;
using System.Reflection;
using log4net;

namespace AlgorithmsCore.Contracts
{
    public abstract class AspgBase
    {
        protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected readonly Options.BaseOptions Options;
        protected readonly IGraph Graph;
        protected readonly Random Rnd;

        protected AspgBase(Options.BaseOptions options, IGraph graph, Random rnd)
        {
            Options = options;
            Graph = graph;
            Rnd = rnd;
        }

        public abstract ResultData GetQuality();

        protected int Roulette(decimal[] probability)
        {
            var boundary = (decimal)Rnd.NextDouble();
            var currentSumOfProbability = 0M;
            for (var i = 0; i < Graph.NumberOfVertices; i++)
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
