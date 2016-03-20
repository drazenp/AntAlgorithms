function dobrota=algn1(inputfile1,inputfile2,MaxBrIteracija,BrRegiona,tol,BrMedjuPodela)
% Primer MedjuRegion1('IK.txt','IK2.txt',10,3,0.1,10)
%---------======I=======----------------------------
fid1 = fopen(inputfile1, 'r');
fid2 = fopen(inputfile2, 'r');
grades1 = textscan(fid1, '%f %f %f', 'headerlines', 1);
grades2 = textscan(fid2, '%f', 'headerlines', 1);
	a=grades1{1};
	b=grades1{2};
	c=grades1{3};
	d=grades2{1};
TC=d';
n2=length(a);
for i=1:n2
	TG((a(i)+1),(b(i)+1))=c(i);
	TG((b(i)+1),(a(i)+1))=c(i);
end
fclose(fid1);
fclose(fid2);
%---------======I=======----------------------------
%%%%%%%%%%%%% Kljucni parametrri%%%%%%%%%
% MaxBrIteracija=100; % broj iteracija
% BrRegiona = 3; % broj mrava = broj regiona
alfa=1; % znacaj debljine feromonskog traga
beta=5; % znacaj povezanosti cvorova
ro=0.45; % faktor isparavanja feromona
% tol = 0.1; % tolerancija
% BrMedjuPodela=10; % broj podela u jednoj iteraciji
%%%%%%%%%%%%% Kljucni parametri %%%%%%%%%

dobrota=ASPG(TC,TG,MaxBrIteracija,BrRegiona,alfa,beta,ro,tol,BrMedjuPodela);

function dobrota=ASPG(TC,TG,MaxBrIteracija,BrRegiona,alfa,beta,ro,tol,BrMedjuPodela)
global ASPGOpcije Problem SistemMrava
tic
	ASPGOpcije = InicializacijaParametara(TG,alfa,beta,ro,MaxBrIteracija,BrRegiona,BrMedjuPodela,tol);
	Problem = InitProblem(TG);
	dozvola = DozvoljenaVelicina(TC);
	IVreme = 0;
	% Snimanje=[]; ISnimi2=[];
	% hold off,clf;
	N=ASPGOpcije.n-ASPGOpcije.h;
	while 1
		SistemMrava = InitSistemMrava(BrMedjuPodela);
		ASPGOpcije.ZauzeteTacke=zeros(BrMedjuPodela,ASPGOpcije.n); % trenutno zauzeti cvorovi
		ASPGOpcije.Korak=zeros(ASPGOpcije.MedjuPodela,ASPGOpcije.h); % prati broj cvorova u regionima
		for i=1:BrMedjuPodela
			for j=1:BrRegiona
				PocetneTacke(i,j);
				ASPGOpcije.Korak(i,j)=ASPGOpcije.Korak(i,j)+1;
			end
	    end
		SumaTezinaTacaka(TC);
		for k=1:BrMedjuPodela
			for mrav = 1:N
				[vrednost,mrav]=min(ASPGOpcije.SumaTezina(k,:));
				korak=ASPGOpcije.Korak(k,mrav)+1;
				ASPGOpcije.Korak(k,mrav)=korak;				
				V=IzracunajVerovatnocu(mrav,korak,k);
				odabrana=Rulet(V);
				ASPGOpcije.ZauzeteTacke(k,odabrana)=odabrana;
				SistemMrava.Putovanja(mrav,korak,k)=odabrana;
				ASPGOpcije.SumaTezina(k,mrav)=ASPGOpcije.SumaTezina(k,mrav)+TC(odabrana);
			end
		end
		IVreme=IVreme+1;
		TezinaGranaPovezanosti();
		SumaKriterijumaOptimalnosti(dozvola,TC);
		GlobalnaPromenaFeromona(dozvola);
		% ISnimi = 
		Rezultati(IVreme);
		% if ISnimi<0
			% ISnimi=-1;
		% end
		% ISnimi2=ASPGOpcije.GNVelicina;
		% if ASPGOpcije.GNVelicina<0
			% ISnimi2=-1;
		% end
		% SnimanjeTrenutne(IVreme)=ISnimi;
		% SnimanjeNajbolje(IVreme)=ISnimi2;
		% ASPGOpcije.SumaTezina=[];
		if Prekini(IVreme)
			break;
		end
	end
	vreme=toc
	dobrota=ASPGOpcije.GNVelicina;
	% ASPGOpcije.GNVelicina % prikaz kvaliteta resenja
    % ASPGOpcije.GNPutanja % prikaz putanje resenja
	% plot(1:length(SnimanjeTrenutne),SnimanjeTrenutne,'b');
	% hold on;
	% plot(1:length(SnimanjeNajbolje),SnimanjeNajbolje,'r');
	% grid on;
	% xlabel('Iteracija');
	% ylabel('Ukupna tezina grana');
%% ---------------------INICIJALIZCIJA ASPGOpcije---------------------
function ASPGOpcije = InicializacijaParametara(TG,alfa,beta,ro,MaxBrIteracija,BrRegiona,BrMedjuPodela,tol)
	ASPGOpcije.n=length(TG(:,1)); % broj cvorova
	ASPGOpcije.h=BrRegiona;
	ASPGOpcije.alfa=alfa;
	ASPGOpcije.beta=beta;
	ASPGOpcije.ro=ro;
	ASPGOpcije.Tol=tol; % tolerancija
	ASPGOpcije.BrRedova=ASPGOpcije.n-ASPGOpcije.h+1; % broj redova matrice putovanja
	ASPGOpcije.MaxBrIteracija = MaxBrIteracija; % maksimalan broj iteracija
	ASPGOpcije.C=0.000001; % vredost kojom inicijalizujemo matricu feromona
	ASPGOpcije.GNVelicina=-Inf; % globalno najveci kriterijum optimalnosti
	ASPGOpcije.GNPutanja=zeros(ASPGOpcije.h,ASPGOpcije.BrRedova); % globalno najbolja putanja
	ASPGOpcije.SumaTezina=[]; % suma tezina za svaki region
	ASPGOpcije.F=[]; % trenutni kriterijum optimalnosti
	ASPGOpcije.SumaKOptimalnosti=[]; % suma kriterijuma optimalnosti za svaku medju podelu
	ASPGOpcije.MedjuPodela=BrMedjuPodela; % broj medju podela u svakoj iteraciji
%% ---------------------INICIJALIZACIJA Problem--------------------------
function Problem =  InitProblem(TG)
global ASPGOpcije
	MatrixTau = (ones(ASPGOpcije.n,ASPGOpcije.n)-eye(ASPGOpcije.n,ASPGOpcije.n))*ASPGOpcije.C; % matrica u kojoj cuvamo vrednosti kolicine feromona na putanjama
Problem = struct('dis',TG,'tau',MatrixTau);
%% ----------------------INICIJALIZACIJA SistemMrava---------------------
function SistemMrava = InitSistemMrava(MedjuRegion)
global ASPGOpcije
	Put=zeros(ASPGOpcije.h,ASPGOpcije.BrRedova,ASPGOpcije.MedjuPodela); % vrste - broj mrava (regiona), za svakog po putanja
	GTezina=zeros(ASPGOpcije.MedjuPodela,ASPGOpcije.h);
	SumaKO=zeros(MedjuRegion,1);
SistemMrava=struct('Putovanja',Put,'Tezine',GTezina,'TezineSum',SumaKO);
%% --------------------------------------------------------------
%  Nasumican odabir sledeecog cvora
function PocetneTacke(medjuRegion,region)
global SistemMrava ASPGOpcije
	zauzete=ASPGOpcije.ZauzeteTacke(medjuRegion,:);
	slobodne=[1:ASPGOpcije.n]; % niz svih tacaka (na pocetku)
	slobodne=slobodne(zauzete(1,:)==0);
	x=randsrc(1,1,slobodne); %random odabir jednog cvora iz niza 'slobodne'
	SistemMrava.Putovanja(region,1,medjuRegion)=x; % dodavanje cvora u matricu putovanja
	ASPGOpcije.ZauzeteTacke(medjuRegion,x)=x; % dodavanje cvora u matricu zauzerih cvorova
%% --------------------------------------------------------------
% Racunanje verovatnoce prelaska u sledeci cvor
function Verovatnoca=IzracunajVerovatnocu(mrav_k,korak_i,MedjuRegion)
global SistemMrava ASPGOpcije Problem
	PoseceniCvorovi = SistemMrava.Putovanja(mrav_k,1:korak_i-1,MedjuRegion); % cvorovi koje je mrav k posetio
	m = length(PoseceniCvorovi);
	d = Problem.tau; % ne menjamo globalne matrice (feromona)
	r = Problem.dis; % ne menjamo globalne matrice (tezine grana)
	zauzete=ASPGOpcije.ZauzeteTacke(MedjuRegion,:); % cvorovi kroz koje su prosli maravi zajedno
	zauzete(PoseceniCvorovi)=0; % poseceni cvorovi svih mrava (sem mrava k)
	prosle=[]; % vektor za smestanje posecenih cvorova (sem mrava k)
	% izdvajamo cvorove koje ne pripadaju regionu (mravu)
	prosle=zauzete(zauzete(1,:)~=0);
	%----- anulianje cvorova koje su posetili ostali mravi
	d(prosle,:)=0; % vrsta feromona tog cvora se anulira
	r(prosle,:)=0; % vrsta tezina grana tog cvora se anulira
	d(:,prosle)=0; % klona feromona tog cvora se anulira
	r(:,prosle)=0; % klona tezina grana tog cvora se anulira
	%----- anulianje cvorova koje je mrav vec posetio
	for k=1:m
		d(PoseceniCvorovi(k),PoseceniCvorovi)=0;
		r(PoseceniCvorovi(k),PoseceniCvorovi)=0;
	end
	% racunanje verovatnoce
	feromoni = zeros(1,ASPGOpcije.n);
	grane = zeros(1,ASPGOpcije.n);
	for i=1:ASPGOpcije.n
		feromon=0;
		for j=1:m
			feromon=feromon+d(PoseceniCvorovi(j),i);
			grane(i) = grane(i) + r(PoseceniCvorovi(j),i);
		end
		feromon=feromon/m; % vise cvorva - srednja vrednost feromona
		feromoni(i)=feromoni(i)+feromon;
		if grane(i)==0
			Verovatnoca(i)=(feromoni(i)^ASPGOpcije.alfa);
		else
			Verovatnoca(i)=(feromoni(i)^ASPGOpcije.alfa)*((grane(i))^ASPGOpcije.beta);
		end		
	end
	% Verovatnoca=(feromoni.^ASPGOpcije.alfa)+((grane).^ASPGOpcije.beta);
	if sum(Verovatnoca)~=0
		Verovatnoca=Verovatnoca/sum(Verovatnoca); % verovatnoca odabira sledece tacke
	end
%% --------------------------------------------------------------
% Ruletom omogucavamo nasumicnost kretanja mrava
function odabrana = Rulet(V)
global ASPGOpcije
	r=rand(1); % nasumicna vrednost u opsegu [0,1]
	sumV=0;
	for k=1:ASPGOpcije.n
		sumV=sumV+V(k); % sumira verovatnocu
		if r<=sumV % ako je r manji od (trenutne) ukupne verovatnoce
			break;
		end
    end
    odabrana=k;
%% --------------------------------------------------------------
% MDV - Maksimalna dozvoljena velicina
function Velicina = DozvoljenaVelicina(TC)
global ASPGOpcije
	sumV = sum(TC);
	Velicina = sumV/ASPGOpcije.h*(1+ASPGOpcije.Tol);
%% --------------------------------------------------------------
% Tezina grana svkog regiona posebno
function TezinaGranaPovezanosti()
global SistemMrava ASPGOpcije Problem
	for k=1:ASPGOpcije.MedjuPodela
		Tezine = zeros(1,ASPGOpcije.h); % za cuvanje tezina regiona
		for l=1:ASPGOpcije.h % za svaki region
			zauzet=ASPGOpcije.ZauzeteTacke(k,:);
			temp=SistemMrava.Putovanja(l,:,k); % cvorovi koje je posetio mrav
			temp=temp(SistemMrava.Putovanja(l,:,k)~=0); % odstranjivanje 0-le
			zauzet(temp)=0; % odstranjivanje prodjenih i skupa zauzetih cvorova
			zauzete=zauzet(zauzet(1,:)~=0);
			% anuliramo sve sem cvorova tog regiona
			r = Problem.dis;
			r(zauzete,:)=0;
			r(:,zauzete)=0;
			% sumiranje tezina povezanosti grana
			SistemMrava.Tezine(k,l)=sum(sum(r))/2;
        end	
	end
%% --------------------------------------------------------------
function SumaTezinaTacaka(TC)
global SistemMrava ASPGOpcije
	for i=1:ASPGOpcije.MedjuPodela
		for l=1:ASPGOpcije.h
			ASPGOpcije.SumaTezina(i,l)=TC(SistemMrava.Putovanja(l,1,i));
		end
	end
%% --------------------------------------------------------------
function SumaKriterijumaOptimalnosti(MDV,TC)
global SistemMrava ASPGOpcije
	for i=1:ASPGOpcije.MedjuPodela		
		for j=1:ASPGOpcije.h
			ASPGOpcije.SumaKOptimalnosti(i,j)=...
			SistemMrava.Tezine(i,j)-1000*(ASPGOpcije.SumaTezina(i,j)-MDV)*(ASPGOpcije.SumaTezina(i,j)>=MDV);		
		end
		SistemMrava.TezineSum(i,1)=sum(ASPGOpcije.SumaKOptimalnosti(i,:));
	end
%% --------------------------------------------------------------
% Promena kolicine feromona
function GlobalnaPromenaFeromona(MDV)
global SistemMrava ASPGOpcije Problem
	[F,MedjuRegion]=max(SistemMrava.TezineSum);
	sumdtau=zeros(ASPGOpcije.n,ASPGOpcije.n); % sumdtau je promena feromona koja ce se obaviti nad poljem
	if F>0 % u slucaju da je F manje od nule ne posipamo feromon
		for mrav=1:ASPGOpcije.h % za svakog mrava
			% B=0.01*(F+1.2*SistemMrava.Tezine(mrav));
			[vrednost,region]=max(SistemMrava.Tezine);
			if mrav==region
				B=0.01*(F+1.2*SistemMrava.Tezine(mrav));
			else
				B=0.01*F;
			end
			temp=SistemMrava.Putovanja(mrav,:,MedjuRegion); % cvorovi koje je posetio mrav
			temp=temp(SistemMrava.Putovanja(mrav,:,MedjuRegion)~=0); % odstranjivanje 0-le
			n=length(temp);
			j=2;
			for i=1:(n-1) % za svaki od prodjenih cvorova tog mrava
				for l=j:n
					sumdtau(SistemMrava.Putovanja(mrav,i,MedjuRegion),SistemMrava.Putovanja(mrav,l,MedjuRegion))=...
					sumdtau(SistemMrava.Putovanja(mrav,i,MedjuRegion),SistemMrava.Putovanja(mrav,l,MedjuRegion))+B;
					sumdtau(SistemMrava.Putovanja(mrav,l,MedjuRegion),SistemMrava.Putovanja(mrav,i,MedjuRegion))=...
					sumdtau(SistemMrava.Putovanja(mrav,l,MedjuRegion),SistemMrava.Putovanja(mrav,i,MedjuRegion))+B;
				end
				j=j+1;
			end
		B=0;
		end
	else
		sumdtau=sumdtau+0.0000001;
	end
	ASPGOpcije.F=F;
	% ro predstavlja faktor isparavanja, a sumdtau promena koja ce se obaviti nad tim poljem.
	% U slucaju da je taj potez deo kvalitetnog rešenja, vrednost ce se povecati 
	% za neku vrednost, dok ce u suprotnom izraz poprimiti vrednost 0
	Problem.tau=Problem.tau*(1-ASPGOpcije.ro)+sumdtau;
%% --------------------------------------------------------------
function Rezultati(IVreme)
global SistemMrava ASPGOpcije Problem
	[F,MedjuRegion]=max(SistemMrava.TezineSum);
	INVelicina=ASPGOpcije.F;
	INPutanja = SistemMrava.Putovanja(:,:,MedjuRegion);
	if INVelicina>=ASPGOpcije.GNVelicina
		ASPGOpcije.GNVelicina = INVelicina; % pamti najbolji kriterijum optimalnosti
		ASPGOpcije.GNPutanja = INPutanja; % pamti najbolju putanju
	end
	GNPutanja = ASPGOpcije.GNPutanja;
	GNVelicina = ASPGOpcije.GNVelicina;
	% Snimi = INVelicina;
%% --------------------------------------------------------------
function flag = Prekini(IVreme)
global ASPGOpcije SistemMrava Problem
	flag = false;
	if IVreme>=ASPGOpcije.MaxBrIteracija
	    flag = true;
	end