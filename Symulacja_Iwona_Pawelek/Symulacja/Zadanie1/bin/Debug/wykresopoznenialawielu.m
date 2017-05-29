data0 = load("lambda65");
data1 = load("lambda67");
data2 = load("lambda69");
data3 = load("lambda71");
data4 = load("lambda73");

%a1=max(length(data0),length(data1));
%a2=max(length(data2),length(data3));
%a=max(a1,a2);
%
%data0=[data0, zeros(1,a-length(data0))]; 
%data1=[data1, zeros(1,a-length(data1))]; 
%data2=[data2, zeros(1,a-length(data2))]; 
%data3=[data3, zeros(1,a-length(data3))]; 

%Rysowanie wykresu opoznienia dla 1000
figure(1);
plot(data0,'r'); axis([0 1000 0 270]); hold on;
plot(data1, 'y'); axis([0 1000 0 270]);hold on;
plot(data2, 'g'); axis([0 1000 0 270]); hold on;
plot(data4, 'b'); axis([0 1000 0 270]); hold on;
plot(data3, 'c'); axis([0 1000 0 270]);
ylabel("Srednie opoznienie pakietu [ms]");
xlabel("Numer odebranego pakietu");
title("Wyznaczanie dlugosci fazy poczatkowej");
legend("lambda = 0.0065", "lambda = 0.0067", "lambda = 0.0069", "lambda = 0.0071","lambda = 0.0073");
legend boxoff

%Rysowanie wykresu dla opoznienia - faza poczatkowa
figure(2);
plot(data0,'r'); axis([0 100 0 100]); hold on;
plot(data1, 'y'); axis([0 100 0 100]);hold on;
plot(data2, 'g'); axis([0 100 0 100]); hold on;
plot(data4, 'b'); axis([0 100 0 100]); hold on;
plot(data3, 'c'); axis([0 100 0 100]);
ylabel("Srednie opoznienie pakietu [ms]");
xlabel("Numer odebranego pakietu");
title("Wyznaczanie dlugosci fazy poczatkowej");
legend("lambda = 0.0065", "lambda = 0.0067", "lambda = 0.0069", "lambda = 0.0071","lambda = 0.0073","north");
legend boxoff
title("Wyznaczanie dlugosci fazy poczatkowej");
grid on