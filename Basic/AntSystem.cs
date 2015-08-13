using System.Collections.Generic;

namespace Basic
{
    public class AntSystem
    {
        /// <summary>
        /// For each colony define it's trail.
        /// </summary>
        //public int[,] Treil;

        public List<HashSet<int>> Treil; 

        /// <summary>
        /// The sum of all vrtices weights for each colony.
        /// </summary>
        public decimal[] SumOfRegionWeight;

        public AntSystem(int numberOfRegions, int maxNumberOfVerticesPerTrail)
        {
            Treil = new List<HashSet<int>>();
            //Treil = new int[numberOfRegions,maxNumberOfVerticesPerTrail];
            for (var i = 0; i < numberOfRegions; i++)
            {
                Treil.Add(new HashSet<int>());
            }

            SumOfRegionWeight = new decimal[numberOfRegions];
        }

        public void InitializeTreils()
        {
            
        }
    }
}
