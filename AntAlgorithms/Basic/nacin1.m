function dobrota=algn1(inputfile1,inputfile2,MaxBrIteracija,BrRegiona,tol)
% % Primer nacin1('Graphs/B1.txt','Graphs/B2.txt',100,3,0.1)
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
% sum(c)
n2=length(a);
for i=1:n2
	TG((a(i)+1),(b(i)+1))=c(i);
	TG((b(i)+1),(a(i)+1))=c(i);
end
fclose(fid1);
fclose(fid2);
%---------======I=======----------------------------
%%%%%%%%%%%% Kljucni parametrri%%%%%%%%%
% MaxBrIteracija=100; % broj iteracija
% BrRegiona = 3; % broj mrava = broj regiona
alfa=1; % znacaj debljine feromonskog traga
beta=5; % znacaj povezanosti cvorova
ro=0.6; % faktor isparavanja feromona
% tol = 0.1; % tolerancija
%%%%%%%%%%%%% Kljucni parametri %%%%%%%%%

dobrota=ASPG(TC,TG,MaxBrIteracija,BrRegiona,alfa,beta,ro,tol);

function kvalitet=ASPG(TC,TG,MaxBrIteracija,BrRegiona,alfa,beta,ro,tol)
global ASPGOpcije Problem SistemMrava
tic
	ASPGOpcije=InicializacijaParametara(TG,alfa,beta,ro,MaxBrIteracija,BrRegiona,tol);
	Problem=InicijalniProblem(TG);
	SistemMrava=InitSistemMrava();
	dozvola=DozvoljenaVelicina(TC);
	IVreme=0;
	% Snimi1=[]; Snimi2=[];	
    % hold off,clf;
	N=ASPGOpcije.n-ASPGOpcije.h;
	while 1
        ASPGOpcije.ZauzeteTacke=zeros(1,ASPGOpcije.n); % niz zauzetih tacaka
        ASPGOpcije.Korak=zeros(ASPGOpcije.h,1); % prati broj cvorova u regionima
		for i=1:BrRegiona
			PocetneTacke(i);
			ASPGOpcije.Korak(i)=ASPGOpcije.Korak(i)+1;
	    end
		% ASPGOpcije.ZauzeteTacke=[1,2,0,4,5,6,7,0,0];
		% SistemMrava.Putovanja(:,1)=[6;4;7;1;2;5];
		SumaTezinaCvorova(TC);
		for j=1:N
			[vrednost,mrav]=min(ASPGOpcije.SumaTezina);
			korak=ASPGOpcije.Korak(mrav)+1;
			ASPGOpcije.Korak(mrav)=korak;
			V=IzracunajVerovatnocu(korak,mrav);
			odabrana=Rulet(V);
			ASPGOpcije.ZauzeteTacke(odabrana)=odabrana;
			SistemMrava.Putovanja(mrav,korak)=odabrana;
			ASPGOpcije.SumaTezina(mrav)=ASPGOpcije.SumaTezina(mrav)+TC(odabrana);
        end       
		IVreme=IVreme+1;
		TezinaGranaPovezanosti();
		SumaKriterijumaOptimalnosti(dozvola,TC);
		GlobalnaPromenaFeromona(dozvola);
        % SistemMrava.Putovanja
        % ASPGOpcije.F
        % Problem.tau
		ISnimi = Rezultati(IVreme);
		% if ISnimi<0
			% ISnimi=-1;
		% end
		% Snimi2=ASPGOpcije.GNVelicina;
		% if ASPGOpcije.GNVelicina<0
			% Snimi2=-1;
		% end
		% Snimi1Trenutne(IVreme)=ISnimi;
		% Snimi1Najbolje(IVreme)=Snimi2;
		if Prekini(IVreme)
			break;
		end
    end
    vreme=toc
	% SistemMrava.Putovanja
	kvalitet=ASPGOpcije.GNVelicina; % prikaz kvaliteta resenja
	% ASPGOpcije.GNPutanja; % prikaz putanje
	% % izcrtava tezinu grana za svaku iteraciju
	% plot(1:length(Snimi1Trenutne),Snimi1Trenutne,'b');
	% hold on;
	% plot(1:length(Snimi1Najbolje),Snimi1Najbolje,'r');
	% grid on;
	% xlabel('Iteracija');
	% ylabel('Ukupna tezina grana');
%% ---------------------INICIJALIZCIJA ASPGOpcije---------------------
function ASPGOpcije = InicializacijaParametara(TG,alfa,beta,ro,MaxBrIteracija,BrRegiona,tol)
	ASPGOpcije.n=length(TG(:,1)); % broj cvorova
	ASPGOpcije.h=BrRegiona;
	ASPGOpcije.alfa=alfa;
	ASPGOpcije.beta=beta;
	ASPGOpcije.ro=ro;
	ASPGOpcije.Tol=tol; % tolerancija
	ASPGOpcije.BrRedova=ASPGOpcije.n-ASPGOpcije.h+1; % broj redova matrice putovanja
	ASPGOpcije.MaxBrIteracija = MaxBrIteracija; % maksimalan broj iteracija
	ASPGOpcije.C=0.00001; % vredost kojom inicijalizujemo matricu feromona
	ASPGOpcije.GNVelicina=-Inf; % globalno najveci kriterijum optimalnosti
	ASPGOpcije.GNPutanja=zeros(ASPGOpcije.h,ASPGOpcije.BrRedova); % globalno najbolja putanja
	ASPGOpcije.SumaTezina=[]; % suma tezina za svaki region
	ASPGOpcije.F=[]; % trenutni kriterijum optimalnosti
	ASPGOpcije.SumaKOptimalnosti=[]; % suma kriterijuma optimalnosti za svaku medju podelu
%% ---------------------INICIJALIZACIJA Problem--------------------------
function Problem = InicijalniProblem(TG)
global ASPGOpcije
	MatrixTau = (ones(ASPGOpcije.n,ASPGOpcije.n)-eye(ASPGOpcije.n,ASPGOpcije.n))*ASPGOpcije.C; % matrica u kojoj cuvamo vrednosti kolicine feromona na putanjama
Problem = struct('dis',TG,'tau',MatrixTau);
%% ----------------------INICIJALIZACIJA SistemMrava---------------------
function SistemMrava = InitSistemMrava()
global ASPGOpcije
	Put = zeros(ASPGOpcije.h,ASPGOpcije.BrRedova); % vrste - broj mrava (regiona), za svakog po putanja
	TezinaGSum = zeros(ASPGOpcije.h,1);
SistemMrava = struct('Putovanja',Put,'Tezine',TezinaGSum);
%% --------------------------------------------------------------
%  Nasumican odabir sledeecog cvora
function PocetneTacke(region)
global SistemMrava ASPGOpcije
	zauzete=ASPGOpcije.ZauzeteTacke;
	slobodne=[1:ASPGOpcije.n]; % niz svih tacaka (na pocetku)
	if sum(zauzete)>0 % ako ima zauzetih tacaka
		slobodne=slobodne(zauzete(1,:)==0);
    else
		SistemMrava.Putovanja = zeros(ASPGOpcije.h,ASPGOpcije.BrRedova);
	end
	x=randsrc(1,1,slobodne); %random odabir jednog cvora iz niza 'slobodne'
	SistemMrava.Putovanja(region,1)=x; % dodavanje cvora u matricu putovanja
	ASPGOpcije.ZauzeteTacke(x)=x; % dodavanje cvora u matricu zauzerih cvorova
%% --------------------------------------------------------------
% Racunanje verovatnoce prelaska u sledeci cvor
function Verovatnoca = IzracunajVerovatnocu(korak_i, mrav_k)
global SistemMrava ASPGOpcije Problem
	PoseceniCvorovi = SistemMrava.Putovanja(mrav_k, 1:korak_i-1); % cvorovi koje je mrav posetio
	m = length(PoseceniCvorovi);
	d = Problem.tau; % ne menjamo globalne matrice (feromona)
	r = Problem.dis; % ne menjamo globalne matrice (tezine grana)
	zauzete=ASPGOpcije.ZauzeteTacke; % cvorovi kroz koje su prosli maravi
	zauzete(PoseceniCvorovi)=0;
	prosle=[]; % cvorovi koje su zauzeli ostali mravi
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
	r=rand(1); % nasumicna vrednosti u opsegu [0,1]
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
	sumV=sum(TC);
	Velicina=sumV/ASPGOpcije.h*(1+ASPGOpcije.Tol);
%% --------------------------------------------------------------
% Tezina grana svkog regiona posebno
function TezinaGranaPovezanosti()
global SistemMrava ASPGOpcije Problem
	for l=1:ASPGOpcije.h % za svaki region posebno
       	zauzet=ASPGOpcije.ZauzeteTacke;
		temp=SistemMrava.Putovanja(l,:); % cvorovi koje je posetio mrav
		temp=temp(SistemMrava.Putovanja(l,:)~=0); % odstranjivanje 0-le
		zauzet(temp)=0; % odstranjivanje prodjenih i skupa zauzetih cvorova
		zauzete=zauzet(zauzet(1,:)~=0);
		% anuliramo sve sem cvorova tog regiona
		r = Problem.dis;
		r(zauzete,:)=0;
		r(:,zauzete)=0;
		% sumiranje tezina povezanosti grana
		SistemMrava.Tezine(l)=sum(sum(r))/2;
	end
%% --------------------------------------------------------------
% Tezina cvorova svkog pocetnog regiona posebno
function SumaTezinaCvorova(TC)
global SistemMrava ASPGOpcije
	for l=1:ASPGOpcije.h
		ASPGOpcije.SumaTezina(l)=TC(SistemMrava.Putovanja(l,1));
	end
%% --------------------------------------------------------------
% Fitnes funkcija
function SumaKriterijumaOptimalnosti(MDV,TC)
global SistemMrava ASPGOpcije
	for j=1:ASPGOpcije.h
		ASPGOpcije.SumaKOptimalnosti(j)=...
		SistemMrava.Tezine(j)-1000*(ASPGOpcije.SumaTezina(j)-MDV)*(ASPGOpcije.SumaTezina(j)>MDV);		
	end
%% --------------------------------------------------------------
% Promena kolicine feromona
function GlobalnaPromenaFeromona(MDV)
global SistemMrava ASPGOpcije Problem
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
%% --------------------------------------------------------------
function Snimi = Rezultati(IVreme)
global SistemMrava ASPGOpcije Problem
	INVelicina=ASPGOpcije.F;
	INPutanja = SistemMrava.Putovanja;
	if INVelicina>=ASPGOpcije.GNVelicina
		ASPGOpcije.GNVelicina = INVelicina; % pamti najbolji kriterijum optimalnosti
		ASPGOpcije.GNPutanja = INPutanja; % pamti najbolju putanju
	end
	GNPutanja = ASPGOpcije.GNPutanja;
	GNVelicina = ASPGOpcije.GNVelicina;
	Snimi = INVelicina;
%% --------------------------------------------------------------
function flag = Prekini(IVreme)
global ASPGOpcije SistemMrava Problem
	flag = false;
	if IVreme>=ASPGOpcije.MaxBrIteracija
	    flag = true;
	end