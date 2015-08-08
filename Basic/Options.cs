namespace Basic
{
    public class Options
    {
        public int MaxNumberOfIterations;
        public readonly int NumberOfRegions;
        public readonly double Alfa;
        public readonly double Beta;
        public readonly double Ro;
        public readonly decimal Delta;

        public Options(int maxNumberOfIterations, int numberOfRegions, double alfa, double beta, double ro, decimal delta)
        {
            MaxNumberOfIterations = maxNumberOfIterations;
            NumberOfRegions = numberOfRegions;
            Alfa = alfa;
            Beta = beta;
            Ro = ro;
            Delta = delta;
        }
    }
}
