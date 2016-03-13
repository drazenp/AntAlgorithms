using System;
using System.Linq;
using log4net;

namespace AlgorithmsCore.Contracts
{
    public abstract class AspgBase
    {
        protected static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

        protected int Roulette(decimal[] probability)
        {
            var boundary = (decimal)Rnd.NextDouble();
            var currentSumOfProbability = 0M;
            for (var i = 0; i < Graph.NumberOfVertices; i++)
            {
                currentSumOfProbability = currentSumOfProbability + probability[i];
                if (boundary <= currentSumOfProbability)
                {
                    return i;
                }
            }

            return Graph.NumberOfVertices - 1;
        }
    }
}
