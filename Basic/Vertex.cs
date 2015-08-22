namespace Basic
{
    public struct Vertex
    {
        public int Index { get; private set; }
        public int Weight { get; private set; }

        public Vertex(int index, int weight)
        {
            Index = index;
            Weight = weight;
        }
    }
}
