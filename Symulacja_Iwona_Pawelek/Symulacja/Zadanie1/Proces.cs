using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Zadanie1
{
    class Proces
    {
        public Proces()
        {
            _faza = 0;
            _wykonano = false;
            _mojeZdarzenie = new Zdarzenie(this);
        }

        virtual public void Execute()
        {
            Console.WriteLine("Process");
        }

        public void Activate(double czas)
        {
            _mojeZdarzenie._czasZdarzenia = Program.zegar+czas;
            _mojeZdarzenie._priorytet = 0;
            Program.agenda.Add(_mojeZdarzenie);
            var agendaPosortowana = Program.agenda.OrderBy(a => a._czasZdarzenia).ThenBy(a => a._priorytet);
            Program.agenda = agendaPosortowana.ToList();
        }

        public void Activate(double czas, int priorytet)
        {
            _mojeZdarzenie._czasZdarzenia = Program.zegar + czas;
            _mojeZdarzenie._priorytet = priorytet;
            Program.agenda.Add(_mojeZdarzenie);
            var agendaPosortowana = Program.agenda.OrderBy(a => a._czasZdarzenia).ThenBy(a => a._priorytet);
            Program.agenda = agendaPosortowana.ToList();
        }


        public int _faza;
        public bool _wykonano;

        private Zdarzenie _mojeZdarzenie;
    }
}
