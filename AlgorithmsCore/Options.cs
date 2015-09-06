namespace AlgorithmsCore
{
    public struct Options
    {
        public int NumberOfIterations;
        public readonly int NumberOfRegions;
        public readonly double Alfa;
        public readonly double Beta;
        public readonly double Ro;
        public readonly double Delta;

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
