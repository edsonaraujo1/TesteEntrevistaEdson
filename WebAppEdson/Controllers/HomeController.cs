using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebAppEdson.Models;

namespace WebAppEdson.Controllers
{
    public class HomeController : Controller
    {
        const string URLApi = "https://www.utyum.com.br/Seguro/Api/api/";
        //const string URLApi = "https://localhost:44386/api/";

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Contato()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult Contato(ContatoEmail contatoEmail)
        //{
        //    //Conteudo Inicio
        //    var url = URLApi;
        //    if (ModelState.IsValid)
        //    {
        //        using (var cliente = new HttpClient())
        //        {
        //            string jsonObjeto = JsonConvert.SerializeObject(contatoEmail);
        //            var content = new StringContent(jsonObjeto, Encoding.UTF8, "application/json");
        //            var resposta = cliente.PostAsync(url + "ContatoEmail/", content);
        //            resposta.Wait();
        //            if (resposta.Result.IsSuccessStatusCode)
        //            {
        //                var retorno = resposta.Result.Content.ReadAsStringAsync();
        //                TempData["Mensagem"] = retorno.Result;
        //            }
        //        }

        //    }

        //    //Conteudo Fim
        //    return View();
        //}
    }
}
