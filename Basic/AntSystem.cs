﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Basic
{
    public class AntSystem
    {
        private readonly Random _rnd;
        private readonly int _numberOfRegions;
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

        public AntSystem(Random rnd, int numberOfRegions, IGraph graph)
        {
            _rnd = rnd;
            _numberOfRegions = numberOfRegions;
            _graph = graph;

            Treil = new List<HashSet<Vertex>>();
            for (var i = 0; i < numberOfRegions; i++)
            {
                Treil.Add(new HashSet<Vertex>());
            }

            FreeVertices = new HashSet<Vertex>(graph.VerticesWeights);

            PassedVertices = new HashSet<Vertex>();

            WeightOfColonies = new int[numberOfRegions];

            EdgesWeightOfColonies = new int[numberOfRegions];
        }

        public void InitializeTreils()
        {
            for (var i = 0; i < _numberOfRegions; i++)
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

        public double[] CalculateOptimalityCriterion(double maxAllowedWeight)
        {
            var optimalityCriterions = new double[_numberOfRegions];
            for (int i = 0; i < _numberOfRegions; i++)
            {
                optimalityCriterions[i] = EdgesWeightOfColonies[i] - 1000 * (WeightOfColonies[i] - maxAllowedWeight);
                if (WeightOfColonies[i] <= maxAllowedWeight)
                {
                    optimalityCriterions[i] *= -1;
                }
            }
            return optimalityCriterions;
        }

        public void UpdatePhermone(double maxAllowedWeight)
        {
            var optimalityCriterions = CalculateOptimalityCriterion(maxAllowedWeight);
            var sumOfOptimalityCriterions = optimalityCriterions.Sum();

            // Colony with heighes weight.
            var colonyWithHeigWeight = Array.IndexOf(WeightOfColonies, WeightOfColonies.Max());

            // If criterions of optimality is less then 0, the minimum of pheromones will be set.
            if (sumOfOptimalityCriterions > 0)
            {
                for (var indexOfRegion = 0; indexOfRegion < _numberOfRegions; indexOfRegion++)
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
                            _graph.PheromoneMatrix[vertex1.Index, vertex2.Index] += pheromoneToSet;
                            _graph.PheromoneMatrix[vertex2.Index, vertex1.Index] += pheromoneToSet;
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
                        _graph.PheromoneMatrix[i, j] += Constants.MinimalVelueOfPheromoneToSet;
                    }
                }
            }
            
            // 
            /*
            	% rh predstavlja faktor isparavanja, a sumdtau promena koja ce se obaviti nad tim poljem.
                % U slucaju da je taj potez deo kvalitetnog re{enja, vrednost ce se povecati 
                % za neku vrednost, dok ce u suprotnom izraz poprimiti vrednost 0
                Problem.tau=Problem.tau*(1-ASPGOpcije.ro)+sumdtau;
            */
        }
        /*
        	F=sum(ASPGOpcije.SumaKOptimalnosti);
	        sumdtau=zeros(ASPGOpcije.n,ASPGOpcije.n); % matrica promene feromona obavljena na prodjenim putanjama
	        if F>0 % u slucaju da je F manje od nule ne posipamo feromon
		        for mrav=1:ASPGOpcije.h % za svamravog mrava - region
			        % B=0.01*(F+1.2*SistemMrava.Tezine(mrav));
			        [vrednost,region]=max(SistemMrava.Tezine);
			        if mrav==region
				        B=0.01*(F+1.2*SistemMrava.Tezine(mrav));
			        else
				        B=0.01*F;
			        end
			        temp=SistemMrava.Putovanja(mrav,:); % cvorovi koje je posetio mrav
			        temp=temp(SistemMrava.Putovanja(mrav,:)~=0); % odstranjivanje 0-le
			        n=length(temp);
			        j=2;
			        for i=1:(n-1) % za svaki od prodjenih cvorova tog mrava
				        for l=j:n
					        sumdtau(SistemMrava.Putovanja(mrav,i),SistemMrava.Putovanja(mrav,l))=...
					        sumdtau(SistemMrava.Putovanja(mrav,i),SistemMrava.Putovanja(mrav,l))+B;
					        sumdtau(SistemMrava.Putovanja(mrav,l),SistemMrava.Putovanja(mrav,i))=...
					        sumdtau(SistemMrava.Putovanja(mrav,l),SistemMrava.Putovanja(mrav,i))+B;
				        end
				        j=j+1;
			        end
		        B=0;
		        end
	        else
		        sumdtau=sumdtau+0.0000001;
	        end
	        ASPGOpcije.F=F;
	        % rh predstavlja faktor isparavanja, a sumdtau promena koja ce se obaviti nad tim poljem.
	        % U slucaju da je taj potez deo kvalitetnog re{enja, vrednost ce se povecati 
	        % za neku vrednost, dok ce u suprotnom izraz poprimiti vrednost 0
	        Problem.tau=Problem.tau*(1-ASPGOpcije.ro)+sumdtau;
        */
    }
}
