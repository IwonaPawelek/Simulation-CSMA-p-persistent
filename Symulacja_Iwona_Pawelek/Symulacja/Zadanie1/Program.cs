using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Zadanie1
{
    class Program
    {
        #region Parametry
        public static Proces obecnyProces;
        public static double zegar=0.0;
        public static List<Zdarzenie> agenda = new List<Zdarzenie>();
        public static double lambda = 0.0067; //0,0066
        public static double czasSymulacji = 200000.0; //200s
        public static int liczbaSymulacji = 10;
        public static int liczbaNadajnikow =10; //4
        public static double PT = 0.6;
        public static int liczbaRetransmisji = 5; //15
        public static int fazaPoczatkowa = 12;
        public static bool logiOnOff = false;
        public static bool symulacjaKrokowa = false;
        #endregion

        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Ziarno.Generuj();
            int liczbaSymulacji = Program.liczbaSymulacji;

            Console.WriteLine("Wybierz opcje wyswietlania informacji o przebiegu programu: ");
            Console.WriteLine("{0} - logi wylaczone");
            Console.WriteLine("{1} - logi wlaczone");
            int zmiennaLogi = int.Parse(Console.ReadLine());
            switch (zmiennaLogi)
            {
                case 0:
                    Program.logiOnOff = false;
                    break;
                case 1:
                    Program.logiOnOff = true;
                    break;
                default:
                    Console.WriteLine("Podano nieoczekiwana liczbe");
                    break;
            }
            Console.WriteLine("Wybierz opcje krokowego wykonywania petli glownej: ");
            Console.WriteLine("{0} - krokowo");
            Console.WriteLine("{1} - ciagle");
            int zmiennaKrokowo = int.Parse(Console.ReadLine());
            switch (zmiennaKrokowo)
            {
                case 0:
                    Program.symulacjaKrokowa = true;
                    break;
                case 1:
                    Program.symulacjaKrokowa=false;
                    break;
                default:
                    Console.WriteLine("Podano nieoczekiwana liczbe");
                    break;
            }
            Console.WriteLine("Wybierz opcje: ");
            Console.WriteLine("{0} - program dla ustalonych w raporcie parametrow");
            Console.WriteLine("{1} - program dla nowych parametrow");
            int zmiennaParametry = int.Parse(Console.ReadLine());
            switch(zmiennaParametry)
            {
                case 0:
                    Console.WriteLine("Rozpoczynam wykonywanie programu, za chwilę na ekranie pojawią się statystyki pierwszej symulacji...");
                    break;
                case 1:
                    Console.WriteLine("Podaj wartosc lambda: ");
                    Program.lambda = double.Parse(Console.ReadLine());
                    Console.WriteLine("Podaj czas symulacji w [ms]: ");
                    Program.czasSymulacji = int.Parse(Console.ReadLine());
                    Console.WriteLine("Podaj liczbe symulacji: ");
                    Program.liczbaSymulacji = int.Parse(Console.ReadLine());
                    Console.WriteLine("Podaj faze poczatkowa jako ilosc pakietow:");
                    Program.fazaPoczatkowa = int.Parse(Console.ReadLine());
                    break;
                default:
                    Console.WriteLine("Podano nieoczekiwana liczbe");
                    break;
            }

            while (liczbaSymulacji > 0)
            {
                Program.zegar = 0;
                Siec siecSymulowana = new Siec();
                while (zegar < Program.czasSymulacji)
                {
                    obecnyProces = agenda.First()._proces;
                    zegar = agenda.First()._czasZdarzenia;
                    agenda.RemoveAt(0);
                    obecnyProces.Execute();
                    if (Program.symulacjaKrokowa) Console.ReadKey();
                }
                Statystyka.Wyswietl();
                Console.WriteLine();
                Statystyka.ZerujStatystyki();
                agenda.Clear();
                --liczbaSymulacji;
            }

            Statystyka.PodsumowanieStatystyk();
            Ziarno.sr.Close();
            Console.WriteLine("Zakonczono");
            Console.ReadKey();
        }
    }
}

