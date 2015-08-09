using System;
using System.Collections.Generic;
using System.Linq;

namespace Basic
{
    public class Aspg
    {
        private Options _options;
        private readonly Graph _graph;

        public Aspg(Options options, Graph graph)
        {
            _options = options;
            _graph = graph;
        }

        public double GetQuality()
        {
            throw new NotImplementedException();
        }

        public decimal GetMaxAllowedWeight(int[] verticesWeights)
        {
            var sumOfVerticesWeightes = verticesWeights.Sum();
            decimal maxAllowedWeight = sumOfVerticesWeightes /_options.NumberOfRegions * (1 + _options.Delta);
            return maxAllowedWeight;
        }

//% MDV - Maksimalna dozvoljena velicina
//function Velicina = DozvoljenaVelicina(TC)
//global ASPGOpcije
//sumV=sum(TC);
//Velicina=sumV/ASPGOpcije.h*(1+ASPGOpcije.Tol);
    }
}
