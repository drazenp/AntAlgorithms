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

        public decimal GetQuality()
        {
            decimal quality = decimal.MinValue;

            while (_options.NumberOfIterations > 0)
            {
                // niz zauzetih tacaka
                int[] zauzeteTacke = new int[_graph.NumberOfVertices];
                // prati broj cvorova u regionima
                int[,] korak = new int[_options.NumberOfRegions, 1];
                //ASPGOpcije.Korak = zeros(ASPGOpcije.h, 1); % prati broj cvorova u regionima

                _options.NumberOfIterations--;
            }

            return quality;
        }

        public decimal GetMaxAllowedWeight(int[] verticesWeights)
        {
            var sumOfVerticesWeightes = verticesWeights.Sum();
            decimal maxAllowedWeight = sumOfVerticesWeightes / (decimal)_options.NumberOfRegions * (1 + _options.Delta);
            return maxAllowedWeight;
        }

        //Nasumican odabir sledeecog cvora
        //        function PocetneTacke(region)
        //global SistemMrava ASPGOpcije
        //    zauzete = ASPGOpcije.ZauzeteTacke;
        //        slobodne=[1:ASPGOpcije.n]; % niz svih tacaka(na pocetku)
        //	if sum(zauzete)>0 % ako ima zauzetih tacaka
        //        slobodne=slobodne(zauzete(1,:)==0);
        //    else
        //		SistemMrava.Putovanja = zeros(ASPGOpcije.h, ASPGOpcije.BrRedova);
        //        end
        //        x = randsrc(1, 1, slobodne); %random odabir jednog cvora iz niza 'slobodne'
        //    SistemMrava.Putovanja(region,1)=x; % dodavanje cvora u matricu putovanja
        //    ASPGOpcije.ZauzeteTacke(x)=x; % dodavanje cvora u matricu zauzerih cvorova
    }
}
