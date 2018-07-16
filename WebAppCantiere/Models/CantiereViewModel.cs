using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebAppCantiere.Models
{
    public class CantiereViewModel
    {
        public string NomeCliente { get; set; }
        public string EmailCliente { get; set; }
        public string Luogo { get; set; }
        public List<IFormFile> Photos { get; set; }
    }
}
