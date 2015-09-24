using System.Collections.Generic;

namespace AlgorithmsCore
{
    public class Result
    {
        private List<HashSet<Vertex>> Trails { get; }
        public double Quality { get; }

        public Result(double quality)
        {
            Quality = quality;
        }

        public Result(double quality, List<HashSet<Vertex>> trails)
        {
            Quality = quality;
            Trails = GetCopyOfTrails(trails);
        }

        public List<HashSet<Vertex>> GetCopyOfTrails(List<HashSet<Vertex>> trails)
        {
            var copyTreils = new List<HashSet<Vertex>>();
            foreach (var treil in trails)
            {
                var copyTreil = new HashSet<Vertex>();
                foreach (var vertex in treil)
                {
                    copyTreil.Add(new Vertex(vertex.Index, vertex.Weight));
                }
                copyTreils.Add(copyTreil);
            }
            return copyTreils;
        }
    }
}
