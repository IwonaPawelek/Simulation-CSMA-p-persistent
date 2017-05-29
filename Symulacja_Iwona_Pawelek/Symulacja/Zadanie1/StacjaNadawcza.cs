using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie1
{
    //Klasa dziedziczy po klasie Stacja. Jest odpowiedzialna
    //za generowanie i przechowywanie wygenerowanych pakietow do 
    //czasu w ktorym nie zostana one transmitowane przez kanal.

    class StacjaNadawcza
    {
        //Konstruktor bez parametrow wejsciowych
        public StacjaNadawcza(int id) 
        {
            Id = id;
            _bufor = new Queue<Pakiet>();
            generatorWykladniczy = new WykladniczyGeneratorLosowy(Program.lambda, Ziarno.PobierzZiarno());
            new Pakiet(Id).Activate(CGPk());
        }

        //zwroci pierwszy pakiet
        public Pakiet pierwszyPakiet()
        {
            return _bufor.First();
        }

        //dodawanie do buforu
        public void DodajDoBuforu(Pakiet pakiet)
        {
            _bufor.Enqueue(pakiet);
        }

        //usowa z buforu
        public void UsunZBuforu()
        {
            _bufor.Dequeue();
        }

        //Ile jest w buforze
        public int IleJestWBuforze()
        {
            return _bufor.Count;
        }

        //losuje wartosc CGPk dla kolejnych pakietow
        public double CGPk()
        {
            return Math.Round(generatorWykladniczy.Rand(), 1);
        }

        //Pole identyfikujace stacje nadawcza z odbiorcza
        public int Id { get; set; }

        //Bufor przechowujący pakiety do transmisji
        private Queue<Pakiet> _bufor;

        //Generator wykladniczy stacji nadawczej
        private WykladniczyGeneratorLosowy generatorWykladniczy;

        #region Statystyka
        //Licznik pakietow nadanych
        public int LiczbaNadanych { get; set; }

        public void ZerujNadajnik()
        {
            LiczbaNadanych = 0;
            _bufor.Clear();
        }
        #endregion

    }
}
