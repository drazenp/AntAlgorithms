using System;
using System.Collections.Generic;
using System.Linq;

namespace Basic
{
    public class AntSystem
    {
        private readonly Random _rnd;
        private readonly int _numberOfRegions;

        /// <summary>
        /// For each colony define it's trail.
        /// </summary>
        public List<HashSet<int>> Treil; 

        /// <summary>
        /// The sum of all vrtices weights for each colony.
        /// </summary>
        public decimal[] SumOfRegionWeight;

        public HashSet<int> FreeVertices { get; private set; }

        public HashSet<int> PassedVertices { get; }

        public AntSystem(Random rnd, int numberOfRegions, IGraph graph)
        {
            _rnd = rnd;
            _numberOfRegions = numberOfRegions;

            Treil = new List<HashSet<int>>();
            for (var i = 0; i < numberOfRegions; i++)
            {
                Treil.Add(new HashSet<int>());
            }

            SumOfRegionWeight = new decimal[numberOfRegions];

            FreeVertices = new HashSet<int>(graph.VerticesWeights);

            PassedVertices = new HashSet<int>();
        }

        public void InitializeTreils()
        {
            for (int i = 0; i < _numberOfRegions; i++)
            {
                var randomFreeVertix = FreeVertices.Shuffle(_rnd).First();
                FreeVertices.Remove(randomFreeVertix);

                Treil[i].Add(randomFreeVertix);

                PassedVertices.Add(randomFreeVertix);
            }
        }



        //Nasumican odabir sledeecog cvora
        //        function PocetneTacke(region)
        //global SistemMrava ASPGOpcije
        //    zauzete = ASPGOpcije.ZauzeteTacke;
        //slobodne=[1:ASPGOpcije.n]; % niz svih tacaka(na pocetku)
        //	if sum(zauzete)>0 % ako ima zauzetih tacaka
        //        slobodne=slobodne(zauzete(1,:)==0);
        //    else
        //		SistemMrava.Putovanja = zeros(ASPGOpcije.h, ASPGOpcije.BrRedova);
        //end
        //x = randsrc(1, 1, slobodne); %random odabir jednog cvora iz niza 'slobodne'
        //    SistemMrava.Putovanja(region,1)=x; % dodavanje cvora u matricu putovanja
        //    ASPGOpcije.ZauzeteTacke(x)=x; % dodavanje cvora u matricu zauzerih cvorova
    }
}
