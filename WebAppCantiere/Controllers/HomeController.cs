using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Lib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using WebAppCantiere.Models;

namespace WebAppCantiere.Controllers
{
    public class HomeController : Controller
    {
        private readonly CantiereRepo _cantiereRepo;
        private readonly IConfiguration _configuration;
        private readonly InterventoRepo _interventoRepo;

        public HomeController(CantiereRepo cantiereRepo, IConfiguration configuration, InterventoRepo interventoRepo)
        {
            _cantiereRepo = cantiereRepo;
            _configuration = configuration;
            _interventoRepo = interventoRepo;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _cantiereRepo.GetAllCantieri());
        }

        [HttpGet]
        public IActionResult CreateCantiere()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateCantiere(CantiereViewModel c)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_configuration["ConnectionStrings:StorageAccountCS"]);
            CloudBlobClient cbc = storageAccount.CreateCloudBlobClient();

            var photoContainer = cbc.GetContainerReference("cantieri");
            Cantiere cant = new Cantiere
            {
                Luogo = c.Luogo,
                EmailCliente = c.EmailCliente,
                NomeCliente = c.NomeCliente,
                UriPhotos = new List<string>()
            };

            foreach (var file in c.Photos)
            {
                var id = Guid.NewGuid();
                var fileExtension = Path.GetExtension(file.FileName);

                var blobName = $"photo/{id}{fileExtension}";

                var blobRef = photoContainer.GetBlockBlobReference(blobName);


                using (var stream = file.OpenReadStream())
                {
                    await blobRef.UploadFromStreamAsync(stream);
                }


                cant.UriPhotos.Add(blobRef.Uri.AbsoluteUri);
            }


             await _cantiereRepo.CreateCantiere(cant);


            return RedirectToAction("Index");
        }



        public async Task<IActionResult> GetCantiereDetails(Cantiere c)
        {
            List<string> uriPhotos = await _cantiereRepo.GetPhotos(c.Id);
            c.UriPhotos = new List<string>();
            foreach (string uri in uriPhotos)
            {
                c.UriPhotos.Add(uri);
            }
            return View(c);
        }

        [HttpGet]
        public IActionResult CreateIntervento(int idCantiere)
        {   
            InterventoViewModel i = new InterventoViewModel();
            i.IdCantiere = idCantiere;

            

            return View(i);
        }


        [HttpPost]
        public async Task<IActionResult> CreateIntervento(InterventoViewModel intervento)
        {
            Intervento i = new Intervento
            {
                IdCantiere = intervento.IdCantiere,
                Note = intervento.Note,
                StimaCosto = intervento.StimaCosto,
                Tipo = intervento.Tipo
            };
            if (intervento.Photo != null)
            {
                CloudStorageAccount storageAccount =
                    CloudStorageAccount.Parse(_configuration["ConnectionStrings:StorageAccountCS"]);
                CloudBlobClient cbc = storageAccount.CreateCloudBlobClient();

                var photoContainer = cbc.GetContainerReference("interventi");
                var id = Guid.NewGuid();
                var fileExtension = Path.GetExtension(intervento.Photo.FileName);

                var blobName = $"photo/{id}{fileExtension}";

                var blobRef = photoContainer.GetBlockBlobReference(blobName);


                using (var stream = intervento.Photo.OpenReadStream())
                {
                    await blobRef.UploadFromStreamAsync(stream);
                }


                i.UriPhoto = blobRef.Uri.AbsoluteUri;
            }
            await _interventoRepo.InsertIntervento(i);

        
              


            return RedirectToAction("GetCantiereDetails", intervento.IdCantiere);
             
        }

        public async Task<IActionResult> ListInterventi()
        {
            return View(await _interventoRepo.GetAllInterventi());
        }

        public async Task<IActionResult> DetailsIntervento(int interventoId)
        {
            return View(await _interventoRepo.GetDetailsIntervento(interventoId));
        }

        public async Task SendMailPreventivo(Intervento intervento)
        {
            var cantiere =  await _cantiereRepo.Get(intervento.IdCantiere);

            var smtpClient = new SmtpClient
            {
                Host = "smtp-mail.outlook.com", // set your SMTP server name here
                Port = 587, // Port 
                EnableSsl = true,
                Credentials = new NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"])
            };

            using (var message = new MailMessage(_configuration["Email:Username"], cantiere.EmailCliente)
            {
                Subject = "Preventivo",
            Body = $"Salve, il preventivo per l'intervento presso {cantiere.Luogo} è di {intervento.StimaCosto}"
            })
            {
                await smtpClient.SendMailAsync(message);
            }
        }



        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

