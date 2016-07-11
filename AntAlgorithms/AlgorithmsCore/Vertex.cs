using System.Collections.Generic;

namespace AlgorithmsCore
{
    public class Vertex
    {
        public int Index { get; private set; }
        public int Weight { get; private set; }

        private List<int> _connectedEdges;
        /// <summary>
        /// The all other connected vertices with the vertex.
        /// </summary>
        public List<int> ConnectedEdges
        {
            get
            {
                return _connectedEdges ?? (_connectedEdges = new List<int>());
            }
            set { _connectedEdges = value; }
        }

        public Vertex(int index, int weight = 1)
        {
            Index = index;
            Weight = weight;
        }
    }
}
