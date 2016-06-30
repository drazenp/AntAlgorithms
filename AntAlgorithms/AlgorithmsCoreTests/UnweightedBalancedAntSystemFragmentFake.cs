using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgorithmsCore;
using AlgorithmsCore.Contracts;
using AlgorithmsCore.Options;

namespace AlgorithmsCoreTests
{
    class UnweightedBalancedAntSystemFragmentFake : UnweightedBalancedAntSystemFragment
    {
        public void SetTreil(int colonyIndex, Vertex vertex)
        {
            base.Treil[colonyIndex].Add(vertex);
        }

        public void ClearTreil()
        {
            Treil = new List<HashSet<Vertex>>();
            for (var i = 0; i < _options.NumberOfRegions; i++)
            {
                Treil.Add(new HashSet<Vertex>());
            }
        }

        public UnweightedBalancedAntSystemFragmentFake(Random rnd, BaseOptions options, IGraph graph) : base(rnd, options, graph)
        {
        }
    }
}
