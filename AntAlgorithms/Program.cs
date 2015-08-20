using Basic;

namespace AntAlgorithms
{
    class Program
    {
        public const string _basicEdges = "B1.txt";
        public const string _basicVeretxWeights = "B2.txt";

        static void Main(string[] args)
        {
            var options = new Options(numberOfIterations: 100, numberOfRegions: 3, alfa: 1, beta: 5, ro: 0.6, delta: 0.1D);
            var graph = new Graph(_basicEdges, _basicVeretxWeights);
            var aspg = new Aspg(options, graph);
            aspg.GetQuality();
        }
    }
}
