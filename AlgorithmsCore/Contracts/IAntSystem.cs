namespace AlgorithmsCore.Contracts
{
    public interface IAntSystem
    {
        void AddFreeVertexToTreil(int indexOfColony, Vertex vertix);

        /// <summary>
        /// Based on overall weight of colony shoose the next one.
        /// The next colony will be with the lowest weight.
        /// </summary>
        /// <returns></returns>
        int GetNextColony();

        double[] CalculateProbability(int nextColony);
        AntSystemFragment UpdatePhermone(double maxAllowedWeight);
    }
}