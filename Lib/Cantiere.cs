using System;
using System.Collections.Generic;
using System.Text;

namespace Lib
{
    public class Cantiere
    {
        public string NomeCliente { get; set; }
        public string EmailCliente { get; set; }
        public string Luogo { get; set; }
        public List<string> UriPhotos { get; set; }
        public int Id { get; set; }
    }
}
