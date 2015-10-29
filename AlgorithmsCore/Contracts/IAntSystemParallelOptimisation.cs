namespace AlgorithmsCore.Contracts
{
    public interface IAntSystemParallelOptimisation
    {
        /// <summary>
        /// Based on overall weight of colony shoose the next one.
        /// The next colony will be with the lowest weight.
        /// </summary>
        /// <param name="interSectionId">The ID of the inter section.</param>
        /// <returns>The ID of the next colony of the inter section.</returns>
        int GetNextColony(short interSectionId);

        double[] CalculateProbability(short interSectionId, int nextColony);
        void AddFreeVertexToTreil(short interSectionId, int indexOfColony, Vertex vertix);

        /// <summary>
        /// Update pheromone on graph based on quality.
        /// </summary>
        /// <param name="maxAllowedWeight"></param>
        /// <returns>The fragment with best quality.</returns>
        AntSystemFragment UpdatePhermone(double maxAllowedWeight);
    }
}