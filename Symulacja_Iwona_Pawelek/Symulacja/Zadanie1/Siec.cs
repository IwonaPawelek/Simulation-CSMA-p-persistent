using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie1
{
    //Klasa zawierająca wszystkie pozostałe klasy systemu
    class Siec
    {
        //Konstruktor, jego wywołanie powoduje utworzenie listy stacji nadawczych i odbiorczych
        //oraz dodanie do list odpowiedniej liczby nadajników i odbiorników. liczba_stacji jest rowna 4
        public Siec()                  
        {
            _stacjeNadawcze = new List<StacjaNadawcza>();
            _stacjeOdbiorcze = new List<Stacja>();
            _kanal = new Lacze();
            _losowanieCTPk = new UniwersalnyGeneratorLosowy(Ziarno.PobierzZiarno());
            _losowaniePT = new UniwersalnyGeneratorLosowy(Ziarno.PobierzZiarno());
            _losowanieR = new UniwersalnyGeneratorLosowy(Ziarno.PobierzZiarno());

            for(int i=0; i<Program.liczbaNadajnikow; i++)
            {
                _stacjeNadawcze.Add(new StacjaNadawcza(i));
                _stacjeOdbiorcze.Add(new Stacja(i));
            }
        }

        //odpowiada za wybór poprawnej stacji po id pakietu i dodanie 
        //do buforu stacji nadawczej
        public static void DodajDoNadawczej(Pakiet pakiet)
        {
            _stacjeNadawcze[pakiet.Id].DodajDoBuforu(pakiet);
        }

        //metoda sprawdza czy bufor nadajnika nie jest pusty
        public static int SprawdzBufor(Pakiet pakiet)
        {
            return _stacjeNadawcze[pakiet.Id].IleJestWBuforze();
        }

        //odpowiada za wybór poprawnej stacji po id pakietu i dodanie
        //do buforu stacji odbiorczej
        public static void DodajDoOdbiorczej(Pakiet pakiet)
        {
            _stacjeOdbiorcze[pakiet.Id].DodajDoBuforu(pakiet);
            ++(_stacjeOdbiorcze[pakiet.Id].LiczbaOdebranych);
        }

        //odpowiada za wybór poprawnej stacji po id pakietu i usuniecie
        //buforu ze stacji nadawczej
        public static void UsunZNadawczej(Pakiet pakiet)
        {
            _stacjeNadawcze[pakiet.Id].UsunZBuforu();
        }

        //sprawdza czy w swojej stacji nadawczej pakiet
        //jest pierwszy
        public static Pakiet PierwszyPakiet(Pakiet pakiet)
        {
            return _stacjeNadawcze[pakiet.Id].pierwszyPakiet();
        }

        //Pole przechowuje liste stacji nadawczych
        private static List<StacjaNadawcza> _stacjeNadawcze;

        //Pole pzechowuje liste stacji odbiorczych
        private static List<Stacja> _stacjeOdbiorcze;

        //Pole reprezentujace kanal transmisyjny
        private Lacze _kanal;

        #region Statystyka
        //Pakiety odebrane
        public static int Odebrane()
        {
            int wynik = 0;
            foreach (Stacja so in _stacjeOdbiorcze)
            {
                wynik += so.LiczbaOdebranych;
            }
            return wynik;

        }

        //Ustawia pole LiczbaStraconych stacji nadawczej pakietu
        public static void StraconePakiety(Pakiet pakiet)
        {
            ++(_stacjeOdbiorcze[pakiet.Id].LiczbaStraconych);
        }

        //Wywołanie funkcji odbornika zliczajacej liczbe retransmisji
        //dla swoich pakietow
        public static void IloscRetransmisji()
        {
            foreach (Stacja so in _stacjeOdbiorcze)
            {
                so.IloscRetransmisji();
            }
        }

        //Dodaje obliczona stope bledu dla kazdego nadajnik do statystyk
        public static void StopaBledu()
        {
            foreach (Stacja so in _stacjeOdbiorcze)
            {
                Statystyka.StopaBledu.Add(((float)so.LiczbaStraconych / ((float)so.LiczbaOdebranych + (float)so.LiczbaStraconych)));
            }
        }

        //Funkcja wywolywana na koncu symulacji, zbiera statystyki danej symulacji
        public static void StatystykiSieci()
        {
            Siec.StopaBledu();
            Siec.IloscRetransmisji();
        }

        public static void ZerujOdbiorniki()
        {
            for (int i = 0; i < Program.liczbaNadajnikow; ++i)
            {
                _stacjeOdbiorcze[i].ZerujOdbiornik();
            }
        }


        //Zeruje wyniki pod koniec symulacji;
        public static void ZerujSiec()
        {
            for (int i = 0; i < Program.liczbaNadajnikow; ++i)
            {
                _stacjeNadawcze[i].ZerujNadajnik();
                _stacjeOdbiorcze[i].ZerujOdbiornik();
            }

            _stacjeNadawcze.Clear();
            _stacjeOdbiorcze.Clear();
        }
        #endregion

        #region GeneratoryDlaPakietow
        //Ustawienie CGPk
        public static double UstawCGPk(Pakiet pakiet)
        {
            return _stacjeNadawcze[pakiet.Id].CGPk();
        }

        //Matoda zwraca czas CTPk dla pakietu
        public static double CzasCTPk()
        {
            return Math.Round(_losowanieCTPk.Rand(1, 10), 1);
        }

        //Matoda zwraca wylosowana wartosc R
        public static double R(Pakiet pakiet)
        {
            double koniecPrzedzialu = (Math.Pow(2, pakiet.LiczbaRetransmisji)) - 1;
            return Math.Round(_losowanieR.Rand(0, (int)koniecPrzedzialu), 1);
        }

        //Metoda zwraca wylosowana wartosc PT
        public static double PT()
        {
            return _losowaniePT.Rand();
        }
        //Pole reprezentuje generator
        private static UniwersalnyGeneratorLosowy _losowanieCTPk;

        //Pole reprezentuje losowanie czasu retransmisji dla pakietu
        private static UniwersalnyGeneratorLosowy _losowanieR;

        //Pole reprezentuje losowanie prawdopodobienstwa PT
        private static UniwersalnyGeneratorLosowy _losowaniePT;
        #endregion
    }
}
