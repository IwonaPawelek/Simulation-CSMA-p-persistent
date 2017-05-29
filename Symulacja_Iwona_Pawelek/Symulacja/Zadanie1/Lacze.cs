using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie1
{
    //Klasa reprezentuje łącze systemu. Będzie zawirala głowny 
    //algorytm decydujacy o zajetosci kanalu, transmisji i 
    //retransmisji.
    class Lacze
    {
        //Konstruktor tworzy liste pakietow dla lacza
        //oraz w momencie utworzenia ustawia kanal na wolny
        public Lacze() 
        {
            _pakietyOczekujace = new List<Pakiet>();
            KanalWolny = true;
        }


        //Dodaje pakiety do listy w kanale
        public static void DodajDoKanalu(Pakiet pakiet)
        {
            _pakietyOczekujace.Add(pakiet);
        }

        //Usuwa pakiety z kanalu
        public static void UsunZKanalu(Pakiet pakiet)
        {
            _pakietyOczekujace.Remove(pakiet);
        }

        //Jezeli lista pakietow oczekujacych na transmisje jest pusta
        //to znaczy ze kolizja nie wystapi
        public static bool Kolizja()
        {
            if (_pakietyOczekujace.Count == 0) return false;
            else return true;
        }

        //Metoda ustawia flagi pakietow informujace o kolizji kiedy ta 
        //wystapila
        public static void UstawFlagiKolizji()
        {
            foreach (Pakiet p in _pakietyOczekujace) p.FlagaKolizji = true;
        }

        //Pole zawiera liste pakietow ktore jednoczesnie ubiegały
        //sie o dostep do lacza. Z wykorzystaniem tej listy
        //bedzie weryfikowany warunek kolizji
        private static List<Pakiet> _pakietyOczekujace;

        //Ustawia stan kanalu
        public static bool KanalWolny { get; set; }

    }
}
