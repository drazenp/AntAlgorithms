using System.Collections.Generic;

namespace AlgorithmsCore
{
    public class Vertex
    {
        public int Index { get; private set; }
        public int Weight { get; private set; }

        public Vertex(int index, int weight = 1)
        {
            Index = index;
            Weight = weight;
        }
    }
}
