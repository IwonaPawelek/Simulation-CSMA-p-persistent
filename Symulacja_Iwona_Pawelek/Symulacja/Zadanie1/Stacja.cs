using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie1
{
    // Klasa reprezentująca stację odbiorczą, odpowiedzialna
    // za przechowywanie oebranych pakietow
    class Stacja
    {
        //Konstruktor przyjmujący jako argument id stacji. Jego wywołanie powoduje 
        //utworzenie nowego bufora, który bedzie przechowywal odebrane pakiety.
        public Stacja(int id)
        {
            Id = id;
            _bufor = new Queue<Pakiet>();
            LiczbaOdebranych = 0;
        }
      
        //metoda odpowiada za dodanie  pakietu do buforu
        public void DodajDoBuforu(Pakiet pakiet)
        {
            _bufor.Enqueue(pakiet);
        }

        //Zlicza liczbe pakietow w buforze stacji odbiorczej
        public int ileJestWBuforze()
        {
            return _bufor.Count();
        }

        //Pole id pozwala połączyć stację odbiorczą z nadawczą
        public int Id { get; set; }

        //Pole umozliwia przechowanie pakietow
        private Queue<Pakiet> _bufor;

        #region Statystyka
        //zliczanie retransmisji pakietow
        public void IloscRetransmisji()
        {
            foreach (Pakiet p in _bufor)
            {
                Statystyka.LiczbaRetransmisji.Add(p.LiczbaRetransmisji);
            }
        }

        public void ZerujOdbiornik()
        {
            LiczbaOdebranych = 0;
            LiczbaStraconych = 0;
            _bufor.Clear();
        }

        //Pole przechowuje liczbe pakietow odebranych
        public int LiczbaOdebranych { get; set; }

        //Pole przechowuje liczbe straconych pakietow
        public int LiczbaStraconych { get; set; }
        #endregion       
    }
}
