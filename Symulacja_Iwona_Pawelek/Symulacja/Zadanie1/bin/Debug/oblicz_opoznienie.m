data0 = load("opoznienie0.txt");
data1 = load("opoznienie1.txt");
data2 = load("opoznienie2.txt");
data3 = load("opoznienie3.txt");
data4 = load("opoznienie4.txt");
data5 = load("opoznienie5.txt");
data6 = load("opoznienie6.txt");
data7 = load("opoznienie7.txt");
data8 = load("opoznienie8.txt");
data9 = load("opoznienie9.txt");

wynik=dod(data0,data1);
wynik=dod(wynik,data2);
wynik=dod(wynik,data3);
wynik=dod(wynik,data4);
wynik=dod(wynik,data5);
wynik=dod(wynik,data6);
wynik=dod(wynik,data7);
wynik=dod(wynik,data8);
wynik=dod(wynik,data9);

zapisz=(wynik/10);

plik = fopen("lambda65", "w",'ieee-le'); # otarcie pliku do zapisu
fprintf(plik, "%f ", zapisz); # wpisanie macierzy mA do pliku dane
fclose(plik); # zamkniêcie pliku

plot(wynik/10);
axis([0, 5000]);