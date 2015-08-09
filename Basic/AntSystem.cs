namespace Basic
{
    public class AntSystem
    {
        /// <summary>
        /// For each colony define it's trail.
        /// </summary>
        public decimal[,] Treil;

        /// <summary>
        /// The sum of all vrtices weights for each colony.
        /// </summary>
        public decimal[] SumOfRegionWeight;

        public AntSystem(int numberOfRegions, int maxNumberOfVerticesPerTrail)
        {
            Treil = new decimal[numberOfRegions,maxNumberOfVerticesPerTrail];
            for (var i = 0; i < numberOfRegions; i++)
            {
                for (var j = 0; j < maxNumberOfVerticesPerTrail; j++)
                {
                    Treil[i, j] = 1;
                } 
            }

            SumOfRegionWeight = new decimal[numberOfRegions];
            for (var i = 0; i < numberOfRegions; i++)
            {
                SumOfRegionWeight[i] = 1;
            }
        }
    }
}
