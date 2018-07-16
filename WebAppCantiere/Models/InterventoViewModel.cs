using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebAppCantiere.Models
{
    public class InterventoViewModel
    {
        public int Id { get; set; }
        public int IdCantiere { get; set; }
        public int Tipo { get; set; } //0 elettrico, 1 idraulico, 2 murario
        public string Note { get; set; }
        public Decimal StimaCosto { get; set; }
        public IFormFile Photo { get; set; } //accetto solo 1 foto
    }
}
