using Authentiction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        //const string URLApi = "https://www.utyum.com.br/Seguro/Api/api/";
        const string URLApi = "https://localhost:44386/api/";
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
        public ActionResult Index(string busca)
        {
            ViewData["Seguro"] = busca;
            Session.Busc_1 = busca;

            var Seguro = new List<Seguro>();

            try
            {
                var authentiction = new AutenticarJWT(URLApi, "Seguro/");
                var resultado = authentiction.Autenticar();

                Seguro = JsonConvert.DeserializeObject<Seguro[]>(resultado).ToList();

                if (!String.IsNullOrEmpty(busca))
                {
                    Seguro = Seguro.FindAll(i => i.Cliente.Contains(busca) || i.CPF.Contains(busca) || i.Veiculo.Contains(busca) || i.Marca.Contains(busca) || i.Modelo.Contains(busca));
                }
                return View(Seguro);
            }
            catch (Exception ex)
            {
                ViewData["error"] = "Ocorreu um Erro: " + ex.Message;
                return View();
            }


        }

        [HttpGet]
        public IActionResult RotativaPDF()
        {
            string busca = Session.Busc_1;
            var Seguro = new List<Seguro>();
            ErrorViewModel.Mensage = "";

            try
            {
                var authentiction = new AutenticarJWT(URLApi, "Seguro/");
                var resultado = authentiction.Autenticar();

                Seguro = JsonConvert.DeserializeObject<Seguro[]>(resultado).ToList();

                if (!String.IsNullOrEmpty(busca))
                {
                    Seguro = Seguro.FindAll(i => i.Cliente.Contains(busca) || i.CPF.Contains(busca) || i.Veiculo.Contains(busca) || i.Marca.Contains(busca) || i.Modelo.Contains(busca));
                }

                var pdf = new ViewAsPdf(Seguro);

                return pdf;

            }
            catch (Exception e)
            {
                ViewData["error"] = "Erro ao Consultar o Resgistro! " + e.Message;
                ErrorViewModel.RError = "Erro ao Consultar o Resgistro!";
                return View();
            }

        }

        // GET: Seguro/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seguro = new Seguro();
            ErrorViewModel.Mensage = "";

            try
            {
                var authentiction = new AutenticarJWT(URLApi, "Seguro/", id.ToString());
                var resultado = authentiction.Autenticar();

                seguro = JsonConvert.DeserializeObject<Seguro>(resultado);

                if (seguro == null)
                {
                    return NotFound();
                }

                return View(seguro);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Erro ao Consultar o Resgistro! " + e.Message;
                ErrorViewModel.RError = "Erro ao Consultar o Resgistro!";
                return View();
            }
            
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
        public IActionResult Create(Seguro seguro)
        {
            if (ModelState.IsValid)
            {
                var authentiction = new AutenticarJWT(URLApi, "Seguro/");
                var resultado = authentiction.AutenticarPost(seguro);
                ErrorViewModel.Mensage = resultado;
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

            var seguro = new Seguro();

            var authentiction = new AutenticarJWT(URLApi, "Seguro/", id.ToString());
            var resultado = authentiction.Autenticar();
            ErrorViewModel.Mensage = resultado;

            seguro = JsonConvert.DeserializeObject<Seguro>(resultado);

            if (seguro == null)
            {
                return NotFound();
            }

            return View(seguro);
        }

        // POST: Seguro/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Seguro seguro)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   var authentiction = new AutenticarJWT(URLApi, "Seguro/");
                   var resultado = authentiction.AutenticarPut(seguro);
                   ErrorViewModel.Mensage = resultado;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeguroExists(seguro.id))
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

            var seguro = new Seguro();

            var authentiction = new AutenticarJWT(URLApi, "Seguro/", id.ToString());
            var resultado = authentiction.Autenticar();
            ErrorViewModel.Mensage = resultado;

            seguro = JsonConvert.DeserializeObject<Seguro>(resultado);

            return View(seguro);
        }

        // POST: Seguro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var authentiction = new AutenticarJWT(URLApi, "Seguro/", id.ToString());
            var resultado = authentiction.AutenticarDel();
            ErrorViewModel.Mensage = resultado;
                        
            return RedirectToAction(nameof(Index));
        }

        private bool SeguroExists(int id)
        {
            var seguro = new List<Seguro>();

            var authentiction = new AutenticarJWT(URLApi, "Seguro/", id.ToString());
            var resultado = authentiction.Autenticar();

            seguro = JsonConvert.DeserializeObject<Seguro[]>(resultado).ToList();

            if (seguro == null)
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
