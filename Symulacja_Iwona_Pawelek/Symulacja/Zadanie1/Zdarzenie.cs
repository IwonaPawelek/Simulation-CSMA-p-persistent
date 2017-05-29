using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie1
{
    class Zdarzenie
    {
        public Zdarzenie(Proces proces)
        {
            _czasZdarzenia = -1.0;
            _proces = proces;
            _priorytet = 0;
        }
        public double _czasZdarzenia;
        public Proces _proces;
        public int _priorytet;

    }
}
