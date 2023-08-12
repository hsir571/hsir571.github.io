clc
clear


ArraySize = 251;
Freq = 100000;
Ts = 1/Freq;            %period
Lp = 37.5E-06;          %transfomer inductance 
Vin = 20;
Vo = linspace(5,30,ArraySize);      %Vout 5V to 30V
Po = zeros(1, ArraySize);           %power 1.5W to 12 W flat line from 20V to 30V 

for i = 1:151
    Po(i) = 0.7*Vo(i) - 2;
end

for i=152:251
    Po(i) = 12;
end


la = 0.1E-3;     %length of air gap
le = 78.6E-3;   %Effective core length
Ae = 97.1E-6;   %Effective core area    
Ve = 7630E-9;   %Effect core volume
AN = 122E-6;
In = 60.5E-3;
Wirep = 1.724E-8;   %wire resistivity for copper
u0 = pi*4E-7;       %permeability of free space
ur = 1670;          %permeability of the core
u = u0 * ur;        %permability total
Kf = 0.65;
D = zeros(1, ArraySize);
Ddash = zeros(1, ArraySize);
Ilppk = zeros(1, ArraySize);
Ilprms = zeros(1, ArraySize);
Ilsrms = zeros(1, ArraySize);
Lploss = zeros(1, ArraySize);
Lsloss = zeros(1, ArraySize);
H = zeros(1, ArraySize);
B = zeros(1, ArraySize);

Rcore = le / (u*Ae);
Rair = la / (u0*Ae);
Re = ((le/ur) + la) / (u0*Ae); %doing this instead of simple addition for double checking purposes

ue = le / (u0*Re*Ae); %relative permeability of the core itself including the air gap

ut = u0*ue; %relative permeability of the transfomer and everything  

Np = 7;   %Ns is the same as well
Ls = (ut*(Np^2)*Ae)/le;     
Lp = Ls;
for i = 1:ArraySize
    D(i) = sqrt((2*Po(i)*Freq*Ls)/(Vin^2)) ;
    Ddash(i) = (Vin*D(i)) / Vo(i) ; 
    Ilppk(i) = (Vin/Lp) *D(i)*Ts;   %same as Is,peak and Im,peak 
    H(i) = (Np*Ilppk(i)) / le;
    B(i) = ut*H(i) ; 
    Ilprms(i) = sqrt(D(i)/3) * Ilppk(i);
    Ilsrms(i) = sqrt(Ddash(i)/3) * Ilppk(i);    %Ils and Ilp have the same inductance and current but different D
end

%, Rdc of lp, Rdc loss, 

skindepth = sqrt(Wirep / (pi*u0*Freq)) ;
parallel = 2;
Aw = parallel*pi*skindepth^2 ;  %i think this is the wire Acu, im not sure how this can be different between Lp and ls
wirelength = Np * 0.0605;  %i think this is the correct one using magnetic axis horizontal coil former 
Rdc = (Wirep*wirelength)/Aw ;   %same for Lp and Ls

for i = 1:ArraySize
    Lploss(i) = Rdc*(Ilprms(i)^2);
    Lsloss(i) = Rdc*(Ilsrms(i)^2);
end


















