﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using WebAppEdson.Data;
using WebAppEdson.Models;

namespace WebAppEdson.Controllers
{
    [Authorize]
    public class SeguroController : Controller
    {
        const string SessionNome = "Buscar";
        private readonly ApplicationDbContext _context;

        public SeguroController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Seguro
        public async Task<IActionResult> Index()
        {
            return View(await _context.Seguro.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> index(string busca)
        {
            ViewData["Seguro"] = busca;
            Session.Busc_1 = busca;

            var Seguro = new List<Seguro>();

            var url = "https://localhost:44359/api/Seguro";
            HttpClient client = new HttpClient();
            var response = client.GetAsync(url).Result;
            
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;

                Seguro = JsonConvert.DeserializeObject<Seguro[]>(results).ToList();
            }
                        
            if (!String.IsNullOrEmpty(busca))
            {
                Seguro = Seguro.FindAll(i => i.Cliente.Contains(busca) || i.CPF.Contains(busca) || i.Veiculo.Contains(busca) || i.Marca.Contains(busca) || i.Modelo.Contains(busca));
            }
            return View(Seguro);
        }

        [HttpGet]
        public async Task<IActionResult> RotativaPDF()
        {
            string busca = Session.Busc_1;
            var Seguro = new List<Seguro>();

            var url = "https://localhost:44359/api/Seguro";
            HttpClient client = new HttpClient();
            var response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;

                Seguro = JsonConvert.DeserializeObject<Seguro[]>(results).ToList();
            }

            if (!String.IsNullOrEmpty(busca))
            {
                Seguro = Seguro.FindAll(i => i.Cliente.Contains(busca) || i.CPF.Contains(busca) || i.Veiculo.Contains(busca) || i.Marca.Contains(busca) || i.Modelo.Contains(busca));
            }

            var pdf = new ViewAsPdf(Seguro);

            return pdf;
        }

        // GET: Seguro/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Seguro = new Seguro();

            var url = "https://localhost:44359/api/Seguro/";
            HttpClient client = new HttpClient();
            var response = client.GetAsync(url + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;

                Seguro = JsonConvert.DeserializeObject<Seguro>(results);
            }
                        
            if (Seguro == null)
            {
                return NotFound();
            }

            return View(Seguro);
        }

        // GET: Seguro/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Seguro/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seguro Seguro)
        {
            if (ModelState.IsValid)
            {
                var url = "https://localhost:44359/api/Seguro/";

                using (var cliente = new HttpClient())
                {
                    string jsonObjeto = JsonConvert.SerializeObject(Seguro);
                    var content = new StringContent(jsonObjeto, Encoding.UTF8, "application/json");
                    var resposta = cliente.PostAsync(url, content);
                    resposta.Wait();
                    if (resposta.Result.IsSuccessStatusCode)
                    {
                        var retorno = resposta.Result.Content.ReadAsStringAsync();
                        Seguro = JsonConvert.DeserializeObject<Seguro>(retorno.Result);
                    }
                }
                
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Seguro/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Seguro = new Seguro();

            var url = "https://localhost:44359/api/Seguro/";
            HttpClient client = new HttpClient();
            var response = client.GetAsync(url + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;

                Seguro = JsonConvert.DeserializeObject<Seguro>(results);
            }
            if (Seguro == null)
            {
                return NotFound();
            }
            return View(Seguro);
        }

        // POST: Seguro/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seguro Seguro)
        {
            if (id != Seguro.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var seguro = new Seguro();
                try
                {
                    var url = "https://localhost:44359/api/Seguro/";

                    using (var cliente = new HttpClient())
                    {
                        string jsonObjeto = JsonConvert.SerializeObject(Seguro);
                        var content = new StringContent(jsonObjeto, Encoding.UTF8, "application/json");
                        var resposta = cliente.PutAsync(url + id, content);
                        resposta.Wait();
                        if (resposta.Result.IsSuccessStatusCode)
                        {
                            var retorno = resposta.Result.Content.ReadAsStringAsync();
                            Seguro = JsonConvert.DeserializeObject<Seguro>(retorno.Result);
                        }
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeguroExists(Seguro.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Seguro/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Seguro = new Seguro();

            var url = "https://localhost:44359/api/Seguro/";
            HttpClient client = new HttpClient();
            var response = client.GetAsync(url + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;

                Seguro = JsonConvert.DeserializeObject<Seguro>(results);
            }
            if (Seguro == null)
            {
                return NotFound();
            }

            return View(Seguro);
        }

        // POST: Seguro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Seguro = new Seguro();

            var url = "https://localhost:44359/api/Seguro/";
            HttpClient client = new HttpClient();
            var response = client.DeleteAsync(url + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;

                Seguro = JsonConvert.DeserializeObject<Seguro>(results);
            }
                        
            return RedirectToAction(nameof(Index));
        }

        private bool SeguroExists(int id)
        {
            var Seguro = new List<Seguro>();

            var url = "https://localhost:44359/api/Seguro/";
            HttpClient client = new HttpClient();
            var response = client.GetAsync(url + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;

                Seguro = JsonConvert.DeserializeObject<Seguro[]>(results).ToList();
            }
            if (Seguro == null)
            {
                return true;
            }
            return false;
        }

        public class CalculaValorSeguro
        {
            private const double margem = 0.03;
            private const double recebe = 0.05;

            public double CalculaSeguro(double VlVeiculo)
            {
                double risk_01 = (VlVeiculo * 5) / (2 * VlVeiculo);
                double risk_02 = (risk_01 / 100) * VlVeiculo;
                double Premio = risk_02 * (1 + margem);
                double valorfinal = recebe * Premio;

                return valorfinal;
            }
        }


    }
}
