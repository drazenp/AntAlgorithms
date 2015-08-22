using System;
using System.Collections.Generic;
using System.Linq;

namespace Basic
{
    public class AntSystem
    {
        private readonly Random _rnd;
        private readonly Options _options;
        private readonly IGraph _graph;

        /// <summary>
        /// For each colony define it's trail.
        /// </summary>
        public List<HashSet<Vertex>> Treil { get; private set; }

        public HashSet<Vertex> FreeVertices { get; private set; }

        public HashSet<Vertex> PassedVertices { get; private set; }

        /// <summary>
        /// The sum of all vrtices weights for each colony.
        /// TODO: Check if should be used decimal insted int.
        /// </summary>
        public int[] WeightOfColonies { get; private set; }

        public int[] EdgesWeightOfColonies { get; private set; }

        public AntSystem(Random rnd, Options options, IGraph graph)
        {
            _rnd = rnd;
            _options = options;
            _graph = graph;

            Treil = new List<HashSet<Vertex>>();
            for (var i = 0; i < _options.NumberOfRegions; i++)
            {
                Treil.Add(new HashSet<Vertex>());
            }
            
            FreeVertices = new HashSet<Vertex>(graph.VerticesWeights);

            PassedVertices = new HashSet<Vertex>();

            WeightOfColonies = new int[_options.NumberOfRegions];

            EdgesWeightOfColonies = new int[_options.NumberOfRegions];
        }

        public void InitializeTreils()
        {
            for (var i = 0; i < _options.NumberOfRegions; i++)
            {
                var randomFreeVertix = FreeVertices.Shuffle(_rnd).First();

                AddFreeVertexToTreil(i, randomFreeVertix);
            }
        }

        public void AddFreeVertexToTreil(int colonyIndex, Vertex vertix)
        {
            FreeVertices.Remove(vertix);

            Treil[colonyIndex].Add(vertix);

            var currentWeightOfColony = WeightOfColonies[colonyIndex];
            WeightOfColonies[colonyIndex] = currentWeightOfColony + vertix.Weight;

            for (int i = 0; i < Treil[colonyIndex].Count; i++)
            {
                EdgesWeightOfColonies[colonyIndex] += _graph.EdgesWeights[i, vertix.Index];
            }

            PassedVertices.Add(vertix);
        }

        /// <summary>
        /// Based on overall weight of colony shoose the next one.
        /// The next colony will be with the lowest weight.
        /// </summary>
        /// <returns></returns>
        public int GetNextColony()
        {
            var colonyWithMinWeight = WeightOfColonies.Min();
            return Array.IndexOf(WeightOfColonies, colonyWithMinWeight);
        }

        public double[] CalculateProbability(int nextColony)
        {
            var numberOfFreeVertices = FreeVertices.Count;
            double[] probability = new double[_graph.NumberOfVertices];

            var numberOfPassedVertices = Treil[nextColony].Count;

            foreach (var freeVertex in FreeVertices)
            {
                var pheromone = 0D;
                var edges = 0;
                foreach (var passedVertex in Treil[nextColony])
                {
                    pheromone += _graph.PheromoneMatrix[passedVertex.Index, freeVertex.Index];
                    edges += _graph.EdgesWeights[passedVertex.Index, freeVertex.Index];
                }
                pheromone /= numberOfPassedVertices;

                if (edges == 0)
                {
                    probability[freeVertex.Index] = Math.Pow(pheromone, _options.Alfa);
                }
                else
                {
                    probability[freeVertex.Index] = Math.Pow(pheromone, _options.Alfa) + Math.Pow(edges, _options.Beta);
                }
            }

            var probabilitySum = probability.Sum();
            for (int i = 0; i < _graph.NumberOfVertices; i++)
            {
                probability[i] = probability[i] / probabilitySum;
            }

            return probability;
        }

        public double[] CalculateOptimalityCriterion(double maxAllowedWeight)
        {
            var optimalityCriterions = new double[_options.NumberOfRegions];
            for (int i = 0; i < _options.NumberOfRegions; i++)
            {
                optimalityCriterions[i] = EdgesWeightOfColonies[i] - 1000 * (WeightOfColonies[i] - maxAllowedWeight);
                if (WeightOfColonies[i] <= maxAllowedWeight)
                {
                    optimalityCriterions[i] *= -1;
                }
                // ASPGOpcije.SumaKOptimalnosti(j)=SistemMrava.Tezine(j) - 1000 * (ASPGOpcije.SumaTezina(j) - MDV) * (ASPGOpcije.SumaTezina(j) > MDV);
            }
            return optimalityCriterions;
        }

        // TODO: The function must be refactored. It should not return sumOfOptimalityCriterions;
        // ..._options.ro is missing; it must be checked if can be implemenet pheromone update in graph class.
        public double UpdatePhermone(double maxAllowedWeight)
        {
            var optimalityCriterions = CalculateOptimalityCriterion(maxAllowedWeight);
            var sumOfOptimalityCriterions = optimalityCriterions.Sum();

            // Colony with heighes weight.
            var colonyWithHeigWeight = Array.IndexOf(WeightOfColonies, WeightOfColonies.Max());

            // If criterions of optimality is less then 0, the minimum of pheromones will be set.
            if (sumOfOptimalityCriterions > 0)
            {
                for (var indexOfRegion = 0; indexOfRegion < _options.NumberOfRegions; indexOfRegion++)
                {
                    double pheromoneToSet;
                    if (indexOfRegion == colonyWithHeigWeight)
                    {
                        pheromoneToSet = 0.01D * (sumOfOptimalityCriterions + 1.2D * WeightOfColonies[colonyWithHeigWeight]);
                    }
                    else
                    {
                        pheromoneToSet = Constants.MinimalVelueOfPheromoneToSet;
                    }

                    var path = Treil[indexOfRegion];
                    foreach (var vertex1 in path)
                    {
                        foreach (var vertex2 in path.Skip(1))
                        {
                            // TODO: 0.1 must be replaced with _options.ro.
                            _graph.PheromoneMatrix[vertex1.Index, vertex2.Index] = _graph.PheromoneMatrix[vertex1.Index, vertex2.Index] * (1 - 0.1) + pheromoneToSet;
                            _graph.PheromoneMatrix[vertex2.Index, vertex1.Index] = _graph.PheromoneMatrix[vertex2.Index, vertex1.Index] * (1 - 0.1) + pheromoneToSet;
                        }
                    }
                }
            }
            else
            {
                for (var i = 0; i < _graph.PheromoneMatrix.GetLength(0); i++)
                {
                    for (var j = 0; j < _graph.PheromoneMatrix.GetLength(1); j++)
                    {
                        // TODO: 0.1 must be replaced with _options.ro.
                        _graph.PheromoneMatrix[i, j] = _graph.PheromoneMatrix[i, j] * (1 - 0.1) + Constants.MinimalVelueOfPheromoneToSet;
                    }
                }
            }

            return sumOfOptimalityCriterions;
            // 
            /*
            	% rh predstavlja faktor isparavanja, a sumdtau promena koja ce se obaviti nad tim poljem.
                % U slucaju da je taj potez deo kvalitetnog re{enja, vrednost ce se povecati 
                % za neku vrednost, dok ce u suprotnom izraz poprimiti vrednost 0
                Problem.tau=Problem.tau*(1-ASPGOpcije.ro)+sumdtau;
            */
        }

        public List<HashSet<Vertex>> GetCopyOfTrails()
        {
            var copyTreils = new List<HashSet<Vertex>>();
            foreach (var treil in Treil)
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
