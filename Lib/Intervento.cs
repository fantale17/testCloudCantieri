using System;
using System.Collections.Generic;
using System.Text;

namespace Lib
{
    public class Intervento
    {
        public int Id { get; set; }
        public int IdCantiere { get; set; }
        public int Tipo { get; set; } //0 elettrico, 1 idraulico, 2 murario
        public string Note { get; set; }
        public Decimal StimaCosto { get; set; }
        public string UriPhoto { get; set; }
    }
}
