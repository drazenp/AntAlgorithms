using System;

namespace Basic
{
    public class Aspg
    {
        private Options _options;
        private readonly Graph _graph;

        public Aspg(Options options, Graph graph)
        {
            _options = options;
            _graph = graph;
        }

        public double GetQuality()
        {
            throw new NotImplementedException();
        }
    }
}
