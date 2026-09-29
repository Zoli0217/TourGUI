using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourGUI
{
    class Versenyzo
    {
        public Versenyzo(int id, string nev, string csapatNev, string nemzetiseg)
        {
            Id = id;
            Nev = nev;
            CsapatNev = csapatNev;
            Nemzetiseg = nemzetiseg;
        }

        public int Id { get; set; }
        public string Nev { get; set; }
        public string CsapatNev { get; set; }
        public string Nemzetiseg { get; set; }
    }
}
