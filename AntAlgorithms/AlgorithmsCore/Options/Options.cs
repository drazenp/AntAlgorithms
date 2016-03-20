namespace AlgorithmsCore.Options
{
    public class Options
    {
        public int NumberOfIterations { get; set; }
        public int NumberOfRegions { get; }
        public double Alfa { get; }
        public double Beta { get; }
        public double Ro { get; }
        public  double Delta { get; }

        public Options(int numberOfIterations, int numberOfRegions, double alfa, double beta, double ro, double delta)
        {
            NumberOfIterations = numberOfIterations;
            NumberOfRegions = numberOfRegions;
            Alfa = alfa;
            Beta = beta;
            Ro = ro;
            Delta = delta;
        }
    }
}
