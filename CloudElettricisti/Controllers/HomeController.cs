using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CloudElettricisti.Models;
using Lib;
using Newtonsoft.Json;

namespace CloudElettricisti.Controllers
{
    public class HomeController : Controller
    {

        private HttpClient client;
        private string _cantieriBaseUrl;

        public HomeController()
        {
            client = new HttpClient();
            _cantieriBaseUrl = "http://localhost:56830/api/";
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(_cantieriBaseUrl + "List/" + 0); //0 = elettricisti
            var json = await response.Content.ReadAsStringAsync()
            ;
            return View(JsonConvert.DeserializeObject<IEnumerable<InterventoEsterno>>(json));
        }

        
        public async Task<IActionResult> Details(int id)
        {
            HttpResponseMessage response = await client.GetAsync(_cantieriBaseUrl + "Details/" + id);
            var json = await response.Content.ReadAsStringAsync()
                ;
            return View(JsonConvert.DeserializeObject<InterventoEsterno>(json));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
