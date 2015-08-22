using Basic;

namespace AntAlgorithms
{
    class Program
    {
        public const string _basicEdges = "Graphs/B2.txt";
        public const string _basicVeretxWeights = "Graphs/B1.txt";

        static void Main(string[] args)
        {
            var options = new Options(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
            var graph = new Graph(_basicEdges, _basicVeretxWeights);
            graph.InitializeGraph();
            var aspg = new Aspg(options, graph);
            aspg.GetQuality();
        }
    }
}
