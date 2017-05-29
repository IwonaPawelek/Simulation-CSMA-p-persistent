using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Zadanie1
{
    static class Statystyka
    {
        #region Zmienne
        //przechowywanie wartosci wylosowanych do rysowania
        //histogramu dla gen rownomiernego dla jednej symulacji
        public static List<double> LosowanieRownomierne = new List<double>();

        //przechowywanie wartosci wylosowanych do rysownia
        //histograu dla gen wykladniczego dla jednej symulacji
        public static List<double> LosowanieWykladnicze = new List<double>();


        //przechowywanie opoznien pakietow dla jednej symulacji
        public static List<double> SrednieOpoznienie = new List<double>();

        //przechowuje srednie opoznienie dla X symulacji
        public static double Opoznienie = 0.0;

        //przechowywanie opoznien pakietow dla jednej symulacji
        public static List<double> SrednieOczekiwanie = new List<double>();

        //przechowuje oczekiwanie dla X symulacji
        public static double Oczekiwanie = 0.0;

        //przechowuje pakietowa stope bledow dla 1 symulcji
        public static List<double> StopaBledu = new List<double>();

        //przechowuje srednia stope bledu dla X symulacji
        public static double SrStopaBledu=0.0;

        //przechowuje pakietowa stope bledow dla 1 symulcji
        public static List<int> LiczbaRetransmisji = new List<int>();

        //przechowuje liczbe retransmisji dla X symulacji
        public static double Retransmisja = 0.0;

        //przechowuje maksymalna stope bledu dla X symulacji
        public static double MaxStBledu = 0.0;

        //przechowuje maksymalna stope bledu dla X symulacji
        public static double Przeplywnosc = 0.0;

        //licznik potrzebny do zapisu nazwy pliku
        public static int licznik = 0;
        #endregion

        #region FazaPoczatkowa
        public static int odliczanieDoFazyPoczatkowej = 0;
        public static bool wyznaczonoFaze = false;
        public static List<double> czasFazyPoczatkowej = new List<double>();

        public static void ZerujPrzedFazaPoczatkowa()
        {
            SrednieOpoznienie.Clear();
            SrednieOczekiwanie.Clear();
            StopaBledu.Clear();
            LiczbaRetransmisji.Clear();
            Siec.ZerujOdbiorniki();
        }
        #endregion

        #region GeneratoryHistogram
        public static void HistogramRownomierny()
        {
            UniwersalnyGeneratorLosowy generator = new UniwersalnyGeneratorLosowy(123);
            StreamWriter rownomierny = new StreamWriter("rownomierny.txt");
            string zapisz = string.Empty;
            for(int i=0; i<10000; i++)
            {
                zapisz += generator.Rand().ToString() + " ";
            }
            rownomierny.Write(zapisz);
            rownomierny.Close();
        }

        public static void HistogramWykladniczy()
        {
            WykladniczyGeneratorLosowy generator = new WykladniczyGeneratorLosowy(Program.lambda, 123);
            StreamWriter wykladniczy = new StreamWriter("wykladniczy.txt");
            string zapisz = string.Empty;
            for (int i = 0; i < 10000; i++)
            {
                zapisz += generator.Rand().ToString() + " ";
            }
            wykladniczy.Write(zapisz);
            wykladniczy.Close();
        }

        //wywoluje metody zapisujace do plikow dane, na podstawie
        //ktorych tworzone sa histogramy
        public static void Histogramy()
        {
            Statystyka.HistogramRownomierny();
            Statystyka.HistogramWykladniczy();
        }
        #endregion

        #region Statystyka
        //Średnia pakietowa stopa bledow usredniona po K odbiornikach
        public static double PakietowaStopaBledow()
        {
            try
            {
                double srednia = (StopaBledu.Sum()) / StopaBledu.Count();
                SrStopaBledu += srednia;
                return srednia;
            }
            catch
            {
                SrStopaBledu += 0.0;
                return 0.0;
            }
        }

        //Maksymalna stopa bledu
        public static double MaxStopaBledu()
        {
            try
            {
                double max = StopaBledu.Max();
                if (MaxStBledu < max) MaxStBledu = max;
                return max;
            }
            catch
            {
                return 0.0;
            }
        }

        //Średnia liczba retransmisji na pakiet
        public static double SrRetransmisji()
        {
            float wartosc = ((float)LiczbaRetransmisji.Sum())/LiczbaRetransmisji.Count();
            Retransmisja += wartosc;
            return wartosc;

        }

        //Średnie opoznienie systemu
        public static double SrOpoznienie()
        {
            ZapiszOpoznienie();
            float wartosc = ((float)SrednieOpoznienie.Sum()) / SrednieOpoznienie.Count();
            Opoznienie += wartosc;
            return wartosc;
        }

        //Średnie opoznienie systemu
        public static double SrOczekiwanie()
        {
            float wartosc = ((float)SrednieOczekiwanie.Sum()) / SrednieOczekiwanie.Count();
            Oczekiwanie += wartosc;
            return wartosc;
        }
        
        //obliczanie przeplywnosci
        public static double PrzeplywnoscSystemu()
        {
            double wynik = ((float)Siec.Odebrane()) / ((Program.czasSymulacji - czasFazyPoczatkowej.Last())/ 1000);
            Przeplywnosc += wynik;
            return wynik;
        }

        //zapisywanie do nowego pliku
        public static void ZapiszOpoznienie()
        {
            string sciezka = "opoznienie" + licznik + ".txt";
            StreamWriter sw = new StreamWriter(sciezka);
            foreach(double liczba in SrednieOpoznienie) sw.Write(liczba+" ");
            sw.Close();
            ++licznik;
        }

        public static double FazaPoczatkowa()
        {
            double wynik = czasFazyPoczatkowej.Sum() / czasFazyPoczatkowej.Count();
            return wynik;

        }

        public static void ZerujStatystyki()
        {
            Siec.ZerujSiec();
            SrednieOpoznienie.Clear();
            SrednieOczekiwanie.Clear();
            StopaBledu.Clear();
            LiczbaRetransmisji.Clear();
            odliczanieDoFazyPoczatkowej = 0;
            wyznaczonoFaze = false;
        }

        #endregion

        #region Wyswietlanie statystyk
        //Wyswitlanie statystyk dla 1 symulacji
        public static void Wyswietl()
        {
           Siec.StatystykiSieci();
           Console.BackgroundColor = ConsoleColor.DarkGray;
           Console.WriteLine("Średnia pakietowa stopa błędu wynosi: {0}", PakietowaStopaBledow());
           Console.WriteLine("Maksymalna pakietowa stopa błedu wynosi: {0}", MaxStopaBledu());
           Console.WriteLine("Średnia liczba retransmisji wynosi: {0}", SrRetransmisji());
           Console.WriteLine("Przepływność systemu wynosi:{0} pakiet/sek", PrzeplywnoscSystemu());
           Console.WriteLine("Średnie opóźnienie wynosi: {0} ms", SrOpoznienie());
           Console.WriteLine("Średnie oczekiwanie wynosi: {0} ms", SrOczekiwanie());
           Console.ResetColor();
        }

        //Wyswietlenie statystyk dla 10 symulacji
        public static void PodsumowanieStatystyk()
        {
            Statystyka.Histogramy();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("PODSUMOWANIE SYSTEMU:");
            Console.WriteLine("Faza poczatkowa trwa do: {0} ms", Math.Round(FazaPoczatkowa(),4));
            Console.WriteLine("Średnia pakietowa stopa błędu wynosi: {0}",Math.Round((SrStopaBledu/Program.liczbaSymulacji),6));
            Console.WriteLine("Maksymalna pakietowa stopa błedu wynosi: {0}",Math.Round(MaxStBledu,6));
            Console.WriteLine("Średnia liczba retransmisji wynosi: {0}",Math.Round((Retransmisja/Program.liczbaSymulacji),4));
            Console.WriteLine("Przepływność systemu wynosi:{0} pakiet/sek",Math.Round((Przeplywnosc/Program.liczbaSymulacji),4));
            Console.WriteLine("Średnie opóźnienie wynosi: {0} ms", Math.Round((Opoznienie/Program.liczbaSymulacji),4));
            Console.WriteLine("Średnie oczekiwanie wynosi: {0} ms",Math.Round((Oczekiwanie/Program.liczbaSymulacji),4));
            Console.ResetColor();
            Console.ReadKey();

        }

        #endregion
    }
}
