using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebAppEdson.Data;
using WebAppEdson.Models;

namespace WebAppEdson.Controllers
{
    [Authorize]
    [EnableCors("CorsPolicy")]
    public class SeguroController : Controller
    {
        const string SessionNome = "Buscar";
        //const string URLApi = "https://www.utyum.com.br/Seguro/Api/api/Seguro/";
        const string URLApi = "https://localhost:44386/api/Seguro/";
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
        public ActionResult index(string busca)
        {
            ViewData["Seguro"] = busca;
            Session.Busc_1 = busca;

            var Seguro = new List<Seguro>();

            try
            {
                var url = URLApi;
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
            catch (Exception ex)
            {
                ViewData["error"] = ex.Message;
                return View();
            }


        }

        [HttpGet]
        public IActionResult RotativaPDF()
        {
            string busca = Session.Busc_1;
            var Seguro = new List<Seguro>();

            var url = URLApi;
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
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Seguro = new Seguro();

            var url = URLApi;
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
        public IActionResult Create(Seguro Seguro)
        {
            if (ModelState.IsValid)
            {
                var url = URLApi;

                using (var cliente = new HttpClient())
                {
                    string jsonObjeto = JsonConvert.SerializeObject(Seguro);
                    var content = new StringContent(jsonObjeto, Encoding.UTF8, "application/json");
                    var resposta = cliente.PostAsync(url, content);
                    resposta.Wait();
                    if (resposta.Result.IsSuccessStatusCode)
                    {
                        var retorno = resposta.Result.Content.ReadAsStringAsync();
                    }
                }

            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Seguro/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Seguro = new Seguro();

            var url = URLApi;
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
        public async Task<IActionResult> Edit(Seguro Seguro)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var url = URLApi;

                    using (var cliente = new HttpClient())
                    {
                        string jsonObjeto = JsonConvert.SerializeObject(Seguro);
                        var content = new StringContent(jsonObjeto, Encoding.UTF8, "application/json");
                        var resposta = cliente.PutAsync(url, content);
                        resposta.Wait();
                        if (resposta.Result.IsSuccessStatusCode)
                        {
                            var retorno = resposta.Result.Content.ReadAsStringAsync();
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
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Seguro = new Seguro();

            var url = URLApi;
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
        public IActionResult DeleteConfirmed(int id)
        {
            var url = URLApi;
            HttpClient client = new HttpClient();
            var response = client.DeleteAsync(url + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SeguroExists(int id)
        {
            var Seguro = new List<Seguro>();

            var url = URLApi;
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
