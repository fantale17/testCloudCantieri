using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAppCantiere.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class ApiController : Controller
    {
        private readonly InterventoRepo _interventoRepo;

        public ApiController(InterventoRepo interventoRepo)
        {
            _interventoRepo = interventoRepo;
        }

        [Route("List/{type}")]
        [HttpGet]
        public async Task<IEnumerable<InterventoEsterno>> GetInterventoFiltered([FromRoute] int type)
        {
            return await _interventoRepo.GetAllInterventiFiltered(type);
        }



        [Route("Details/{idIntervento}")]
        [HttpGet]
        public async Task<InterventoEsterno> GetInterventoDetails([FromRoute] int idIntervento)
        {
            return await _interventoRepo.GetDatiInterventoEsterno(idIntervento);
        }



    }
}