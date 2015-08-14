using System;
using System.Collections.Generic;

namespace Basic
{
    public class AntSystem
    {
        /// <summary>
        /// For each colony define it's trail.
        /// </summary>
        //public int[,] Treil;

        public List<HashSet<int>> Treil; 

        /// <summary>
        /// The sum of all vrtices weights for each colony.
        /// </summary>
        public decimal[] SumOfRegionWeight;

        public HashSet<int> freeVertices { get; private set; }

        public HashSet<int> passedVertices { get; }

        public AntSystem(Random rnd, int numberOfRegions, int maxNumberOfVerticesPerTrail)
        {
            Treil = new List<HashSet<int>>();
            //Treil = new int[numberOfRegions,maxNumberOfVerticesPerTrail];
            for (var i = 0; i < numberOfRegions; i++)
            {
                Treil.Add(new HashSet<int>());
            }

            SumOfRegionWeight = new decimal[numberOfRegions];
        }

        public void InitializeTreils()
        {
            
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
