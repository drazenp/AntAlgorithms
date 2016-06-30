namespace AlgorithmsCore.Options
{
    public class OptionsParallelOptimisation : BaseOptions
    {
        public short NumberOfInterSections { get; }

        public OptionsParallelOptimisation(int numberOfIterations, int numberOfRegions, double alfa, 
                                            double beta, double ro, double delta, short numberOfInterSections) 
            : base (numberOfIterations, numberOfRegions, alfa, beta, ro, delta)
        {
            NumberOfInterSections = numberOfInterSections;
        }
    }
}
