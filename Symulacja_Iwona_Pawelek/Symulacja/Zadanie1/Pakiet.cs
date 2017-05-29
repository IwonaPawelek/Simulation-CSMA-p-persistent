using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Zadanie1
{
    //Klasa reprezentujca pakiet. 
    class Pakiet:Proces
    {
        //Konstruktor z parametrem wejsciowym id
        public Pakiet(int id)
        {
            Id = id;
            LiczbaRetransmisji = 0;
            FlagaKolizji = false;
            ACK = false;
            CzasCTPk = Siec.CzasCTPk();
        }

        //Metoda zwraca ile było retransmisji danego pakietu
        public int LiczbaRetransmisji { get; set; }

        //Metoda przechowuje czas transmisji z k-tej stacji nadawczej
        //do k-tej stacji odbiorczej
        public double CzasCTPk { get; set; }

        //Metoda zwraca i zapisuje id pakietu
        public int Id { get; set; }

        //Metoda ustawia flagę na true w przypadku wystapienia kolizji
        //przy transmisji
        public bool FlagaKolizji { get; set; }

        //Flaga ACK
        public bool ACK { get; set; }

        #region Statystyka
        //Czas pojawienia się w buforze
        public double CzasPojawienia { get; private set; }

        //Czas od momentu nadawania
        public double CzasNadania { get; private set; }

        //Czas poprawnego odbioru pakietu
        public double CzasOdbioru { get; private set; }
        
        #endregion


        override public void Execute()
        {
            bool active = true;
            while(active)
            {
                switch(_faza)
                {
                    //Faza 0 - aktywacja kolejnego pakietu, ustawienie go w buforze, przechodzi do 
                    //fazy 1
                    case 0:
                        //tworzony jest nowy pakiet ktory zostanie dodany do buforu w chwili aktywacji
                        new Pakiet(this.Id).Activate(Siec.UstawCGPk(this));
                        //dodanie istniejacego do buforu
                        Siec.DodajDoNadawczej(this);
                        CzasPojawienia = Program.zegar;
                        if(Program.logi) Console.WriteLine("Pakiet id {0} aktywuje kolejny pakiet, liczba pakietow w stacji nadawczej {1}, czas {2}", this.Id, Siec.SprawdzBufor(this), Program.zegar);
                        _faza = 1;
                        
                        break;

                    //Faza 1 - sprawdza czy pakiet jest pierwszy w buforze, jesli
                    //jest przechodzi do kolejnej fazy
                    case 1:
                        if (Siec.PierwszyPakiet(this)==this)
                        {
                            if (Program.logi) Console.WriteLine("Pakiet id {0} jest pierwszy w nadajniku, czas {1}", this.Id, Program.zegar);
                            CzasNadania = Program.zegar;
                            Statystyka.SrednieOczekiwanie.Add(CzasNadania - CzasPojawienia); 
                            _faza = 2;
                        }
                        else active = false;
                        break;

                    //Faza 2 - nasluchuje kanal co 0.5ms, jezeli kanal
                    //jest wolny to przechodzi do fazy 3
                    case 2:
                        if(Program.logi) Console.WriteLine("Pakiet id {0} sprawdza stan kanalu, czas {1}", this.Id, Program.zegar);
                        if (!Lacze.KanalWolny)
                            {
                                if (Program.logi) Console.WriteLine("Pakiet id {0} oczekuje na zwolnienie kanalu co 0.5ms, czas {1}", this.Id, Program.zegar);
                                this.Activate(0.5);
                                active = false;
                            }
                            else _faza = 3;
                        break;
                    //Faza 3 - jezeli kanal jest wolny losuje liczbe i 
                    //sprawdza jej prawdopodobienstwo
                    case 3:
                        if (Siec.PT() <= Program.PT)
                        {
                            if (Program.logi) Console.WriteLine("Pakiet id {0} ma p-stw mniejsze od 0.6 rozpoczyna transmisje, czas {1}", this.Id, Program.zegar);
                            _faza = 5;
                        }
                        else
                        {
                            //wait a slot
                            if (Program.zegar % 1 == 0) //czeka 1ms do szczeliny
                            {
                                if (Program.logi) Console.WriteLine("Pakiet id {0} ma p-stwo wieksze niz 0.6 wiec oczekuje do szczeliny, czas {1}", this.Id, Program.zegar);
                                _faza = 4;
                                this.Activate(1.0);
                                active = false;
                            }
                            else
                            {
                                if (Program.logi) Console.WriteLine("Pakiet id {0} ma p-stwo wieksze niz 0.6 wiec oczekuje do szczeliny, czas {1}", this.Id, Program.zegar);
                                _faza = 4;
                                this.Activate(1 - (Program.zegar % 1)); //odczekuje do szczeliny
                                active = false;
                            }
                        }
                        break;

                    case 4:
                        if (!Lacze.KanalWolny)
                        {
                            if (Program.logi) Console.WriteLine("Pakiet id {0} sprawdza czy kanal jest wolny 1ms i wraca do odpytywania co 0.5ms, czas {1}", this.Id, Program.zegar);
                            _faza = 2; //wraca do odpytywania co 0.5ms
                            this.Activate(1.0);
                            active = false;
                        }
                        else
                        {
                            if (Program.logi) Console.WriteLine("Pakiet id {0} wraca do losowania liczby, czas {1}", this.Id, Program.zegar);
                            _faza = 3;
                        }
                        break;

                    //faza 5 - sprawdzanie kanalu i kolizji. Jesli wystapila przechodzi do fazy 6,
                    //jesli nie, dodaje pakiet do kanalu i rozpoczyna transmisje
                    case 5:
                        if (Program.zegar % 1 == 0)
                        {
                            if (Program.logi) Console.WriteLine("Pakiet id {0} rozpoczal transmisje w pelnej szczelinie, czas {1}. Planowany czas transmisji {2}", this.Id, Program.zegar,this.CzasCTPk);
                            if (Lacze.Kolizja())
                            {
                                if (Program.logi) Console.WriteLine("Pakiet id {0} wykryl kolizje, czas {1}", this.Id, Program.zegar);
                                Lacze.UstawFlagiKolizji();
                                this.FlagaKolizji = true;
                            }
                            
                            Lacze.DodajDoKanalu(this);
                            _faza = 6;
                            this.Activate(0.0, 1);
                            active = false;
                        }
                        else //jesli nie to odczekaj czas do następnej szczeliny i ponownie sprawdz czy kolizja
                        {
                            if (Program.logi) Console.WriteLine("Pakiet id {0} czeka na poczatek szczeliny, czas {1}", this.Id, Program.zegar);
                            this.Activate((1-(Program.zegar % 1)));
                            active = false;
                        }
                        break;
                    //Faza 6 - transmisja
                    case 6:
                        if (Program.logi) Console.WriteLine("Pakiet id {0} zajal kanal, czas {1}", this.Id, Program.zegar);
                        Lacze.KanalWolny = false;
                        _faza = 8;
                        this.Activate(CzasCTPk);
                        active = false;
                        break;

                    //Faza 7 - retransmisja
                    case 7:
                        if (Program.logi) Console.WriteLine("Pakiet id {0} rozpoczyna retransmisje, czas {1}", this.Id, Program.zegar);
                        ++this.LiczbaRetransmisji;
                        if(this.LiczbaRetransmisji <Program.liczbaRetransmisji)
                        {
                            double czas = Siec.R(this)*CzasCTPk;
                            this.FlagaKolizji = false;
                            _faza = 2;
                            this.Activate(czas);
                            active = false;
                        }
                        else
                        {
                            //usuwa pakiet z systemu
                            if (Program.logi) Console.WriteLine("Przekroczono LR wiec usuwam pakiet z systemu");
                            Siec.StraconePakiety(this);
                            Siec.UsunZNadawczej(this);
                            if (Siec.SprawdzBufor(this) != 0) Siec.PierwszyPakiet(this).Activate(0.0);
                            active = false;
                        }

                        break;
                    //Faza 8 - sprawdzenie czy wystąpiła kolizja
                    case 8:
                        if(this.FlagaKolizji)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            if (Program.logi) Console.WriteLine("Pakiet id {0} wystaplia kolizja, czas {1}", this.Id, Program.zegar);
                            Console.ResetColor();
                            Lacze.UsunZKanalu(this);
                            if(!Lacze.Kolizja()) Lacze.KanalWolny = true;
                            _faza = 7;
                        }
                        else
                        {
                            _faza = 9;
                            ACK = true;
                            Activate(1.0);
                            active = false;
                        }
                        break;
                    //Faza 9 - odbior pakietu
                    case 9:
                        Siec.DodajDoOdbiorczej(this);
                        if (!Statystyka.wyznaczonoFaze)
                        {
                            ++Statystyka.odliczanieDoFazyPoczatkowej;
                            Statystyka.ZerujPrzedFazaPoczatkowa();
                            if (Statystyka.odliczanieDoFazyPoczatkowej == Program.fazaPoczatkowa)
                            {
                                Statystyka.czasFazyPoczatkowej.Add(Program.zegar);
                                Statystyka.wyznaczonoFaze = true;
                            }
                        }
                        this.CzasOdbioru = Program.zegar;
                        Statystyka.SrednieOpoznienie.Add((CzasOdbioru - CzasPojawienia));
                        Lacze.UsunZKanalu(this);
                        Siec.UsunZNadawczej(this);
                        Lacze.KanalWolny = true;
                        _wykonano = true;
                        if (Siec.SprawdzBufor(this) != 0) Siec.PierwszyPakiet(this).Activate(0.0);
                        active = false;
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        if (Program.logi) Console.WriteLine("Pakiet id {0} zapisal sie w stacji odbiorczej, czas {1}", this.Id, Program.zegar);
                        Console.ResetColor();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
