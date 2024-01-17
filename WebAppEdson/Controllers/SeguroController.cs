using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
                var url = URLApi;

                // Envio da requisição a fim de autenticar
                // e obter o token de acesso
                var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile($"appsettings.json");
                var config = builder.Build();

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage respToken = client.PostAsync(
                       URLApi + "Auth", new StringContent(
                            JsonConvert.SerializeObject(new
                            {
                                Email = config.GetSection("API_Access:UserID").Value,
                                PasswordHash = config.GetSection("API_Access:UserPWD").Value,
                                AccessKey = config.GetSection("API_Access:AccessKey").Value
                            }), Encoding.UTF8, "application/json")).Result;

                    string conteudo =
                        respToken.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(conteudo);

                    if (respToken.StatusCode == HttpStatusCode.OK)
                    {
                        TokenID token = JsonConvert.DeserializeObject<TokenID>(conteudo);
                        if (token.Token != null)
                        {
                            // Associar o token aos headers do objeto
                            // do tipo HttpClient
                            client.DefaultRequestHeaders.Authorization =
                                new AuthenticationHeaderValue("Bearer", token.Token);

                            //HttpClient client = new HttpClient();
                            var response = client.GetAsync(url + "Seguro/").Result;

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
                    }
                   
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

            // Envio da requisição a fim de autenticar
            // e obter o token de acesso
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile($"appsettings.json");
            var config = builder.Build();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage respToken = client.PostAsync(
                   URLApi + "Auth", new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            Email = config.GetSection("API_Access:UserID").Value,
                            PasswordHash = config.GetSection("API_Access:UserPWD").Value,
                            AccessKey = config.GetSection("API_Access:AccessKey").Value
                        }), Encoding.UTF8, "application/json")).Result;

                string conteudo =
                    respToken.Content.ReadAsStringAsync().Result;
                Console.WriteLine(conteudo);

                if (respToken.StatusCode == HttpStatusCode.OK)
                {
                    TokenID token = JsonConvert.DeserializeObject<TokenID>(conteudo);
                    if (token.Token != null)
                    {
                        client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", token.Token);

                        //Conteudo Inicio

                        var response = client.GetAsync(url + "Seguro/").Result;

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

                        //Conteudo Fim
                        return pdf;

                    }
                }

            }
            return Ok();

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

            // Envio da requisição a fim de autenticar
            // e obter o token de acesso
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile($"appsettings.json");
            var config = builder.Build();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage respToken = client.PostAsync(
                   URLApi + "Auth", new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            Email = config.GetSection("API_Access:UserID").Value,
                            PasswordHash = config.GetSection("API_Access:UserPWD").Value,
                            AccessKey = config.GetSection("API_Access:AccessKey").Value
                        }), Encoding.UTF8, "application/json")).Result;

                string conteudo =
                    respToken.Content.ReadAsStringAsync().Result;
                Console.WriteLine(conteudo);

                if (respToken.StatusCode == HttpStatusCode.OK)
                {
                    TokenID token = JsonConvert.DeserializeObject<TokenID>(conteudo);
                    if (token.Token != null)
                    {
                        client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", token.Token);

                        //Conteudo Inicio

                        var response = client.GetAsync(url + "Seguro/" + id).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var results = response.Content.ReadAsStringAsync().Result;

                            Seguro = JsonConvert.DeserializeObject<Seguro>(results);
                        }

                        if (Seguro == null)
                        {
                            return NotFound();
                        }

                        //Conteudo Fim
                    }
                }
                else
                {
                    ErrorViewModel.RError = "Erro ao Consultar o Resgistro!";
                }

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

                // Envio da requisição a fim de autenticar
                // e obter o token de acesso
                var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile($"appsettings.json");
                var config = builder.Build();

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage respToken = client.PostAsync(
                       URLApi + "Auth", new StringContent(
                            JsonConvert.SerializeObject(new
                            {
                                Email = config.GetSection("API_Access:UserID").Value,
                                PasswordHash = config.GetSection("API_Access:UserPWD").Value,
                                AccessKey = config.GetSection("API_Access:AccessKey").Value
                            }), Encoding.UTF8, "application/json")).Result;

                    string conteudo =
                        respToken.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(conteudo);

                    if (respToken.StatusCode == HttpStatusCode.OK)
                    {
                        TokenID token = JsonConvert.DeserializeObject<TokenID>(conteudo);
                        if (token.Token != null)
                        {
                            client.DefaultRequestHeaders.Authorization =
                                new AuthenticationHeaderValue("Bearer", token.Token);

                            //Conteudo Inicio

                            string jsonObjeto = JsonConvert.SerializeObject(Seguro);
                            var content = new StringContent(jsonObjeto, Encoding.UTF8, "application/json");
                            var resposta = client.PostAsync(url + "Seguro/", content);
                            resposta.Wait();
                            if (resposta.Result.IsSuccessStatusCode)
                            {
                                var retorno = resposta.Result.Content.ReadAsStringAsync();
                            }

                            //Conteudo Fim
                        }
                    }
                    else
                    {
                        ErrorViewModel.RError = "Erro ao Incluir o Resgistro!";
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

            // Envio da requisição a fim de autenticar
            // e obter o token de acesso
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile($"appsettings.json");
            var config = builder.Build();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage respToken = client.PostAsync(
                   URLApi + "Auth", new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            Email = config.GetSection("API_Access:UserID").Value,
                            PasswordHash = config.GetSection("API_Access:UserPWD").Value,
                            AccessKey = config.GetSection("API_Access:AccessKey").Value
                        }), Encoding.UTF8, "application/json")).Result;

                string conteudo =
                    respToken.Content.ReadAsStringAsync().Result;
                Console.WriteLine(conteudo);

                if (respToken.StatusCode == HttpStatusCode.OK)
                {
                    TokenID token = JsonConvert.DeserializeObject<TokenID>(conteudo);
                    if (token.Token != null)
                    {
                        client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", token.Token);

                        //Conteudo Inicio

                        var response = client.GetAsync(url + "Seguro/" + id).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var results = response.Content.ReadAsStringAsync().Result;

                            Seguro = JsonConvert.DeserializeObject<Seguro>(results);
                        }
                        if (Seguro == null)
                        {
                            return NotFound();
                        }

                        //Conteudo Fim
                    }
                }
                else
                {
                    ErrorViewModel.RError = "Erro ao Editar o Resgistro!";
                }

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

                    // Envio da requisição a fim de autenticar
                    // e obter o token de acesso
                    var builder = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile($"appsettings.json");
                    var config = builder.Build();

                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage respToken = client.PostAsync(
                           URLApi + "Auth", new StringContent(
                                JsonConvert.SerializeObject(new
                                {
                                    Email = config.GetSection("API_Access:UserID").Value,
                                    PasswordHash = config.GetSection("API_Access:UserPWD").Value,
                                    AccessKey = config.GetSection("API_Access:AccessKey").Value
                                }), Encoding.UTF8, "application/json")).Result;

                        string conteudo =
                            respToken.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(conteudo);

                        if (respToken.StatusCode == HttpStatusCode.OK)
                        {
                            TokenID token = JsonConvert.DeserializeObject<TokenID>(conteudo);
                            if (token.Token != null)
                            {
                                client.DefaultRequestHeaders.Authorization =
                                    new AuthenticationHeaderValue("Bearer", token.Token);

                                //Conteudo Inicio

                                string jsonObjeto = JsonConvert.SerializeObject(Seguro);
                                var content = new StringContent(jsonObjeto, Encoding.UTF8, "application/json");
                                var resposta = client.PutAsync(url + "Seguro/", content);
                                resposta.Wait();
                                if (resposta.Result.IsSuccessStatusCode)
                                {
                                    var retorno = resposta.Result.Content.ReadAsStringAsync();
                                }

                                //Conteudo Fim
                            }
                        }
                        else
                        {
                            ErrorViewModel.RError = "Erro ao Editar o Resgistro!";
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

            // Envio da requisição a fim de autenticar
            // e obter o token de acesso
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile($"appsettings.json");
            var config = builder.Build();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage respToken = client.PostAsync(
                   URLApi + "Auth", new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            Email = config.GetSection("API_Access:UserID").Value,
                            PasswordHash = config.GetSection("API_Access:UserPWD").Value,
                            AccessKey = config.GetSection("API_Access:AccessKey").Value
                        }), Encoding.UTF8, "application/json")).Result;

                string conteudo =
                    respToken.Content.ReadAsStringAsync().Result;
                Console.WriteLine(conteudo);

                if (respToken.StatusCode == HttpStatusCode.OK)
                {
                    TokenID token = JsonConvert.DeserializeObject<TokenID>(conteudo);
                    if (token.Token != null)
                    {
                        client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", token.Token);

                        //Conteudo Inicio

                        var response = client.GetAsync(url + "Seguro/" + id).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var results = response.Content.ReadAsStringAsync().Result;

                            Seguro = JsonConvert.DeserializeObject<Seguro>(results);
                        }
                        if (Seguro == null)
                        {
                            return NotFound();
                        }

                        //Conteudo Fim
                    }
                }
                else
                {
                    ErrorViewModel.RError = "Erro ao Excluir o Resgistro!";
                }

            }
            return View(Seguro);
        }

        // POST: Seguro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var url = URLApi;

            // Envio da requisição a fim de autenticar
            // e obter o token de acesso
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile($"appsettings.json");
            var config = builder.Build();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage respToken = client.PostAsync(
                   URLApi + "Auth", new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            Email = config.GetSection("API_Access:UserID").Value,
                            PasswordHash = config.GetSection("API_Access:UserPWD").Value,
                            AccessKey = config.GetSection("API_Access:AccessKey").Value
                        }), Encoding.UTF8, "application/json")).Result;

                string conteudo =
                    respToken.Content.ReadAsStringAsync().Result;
                Console.WriteLine(conteudo);

                if (respToken.StatusCode == HttpStatusCode.OK)
                {
                    TokenID token = JsonConvert.DeserializeObject<TokenID>(conteudo);
                    if (token.Token != null)
                    {
                        client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", token.Token);

                        //Conteudo Inicio

                        var response = client.DeleteAsync(url + "Seguro/" + id).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var results = response.Content.ReadAsStringAsync().Result;
                        }

                        //Conteudo Fim
                    }
                }
                else
                {
                    ErrorViewModel.RError = "Erro ao Excluir o Resgistro!";
                }

            }
            return RedirectToAction(nameof(Index));
        }

        private bool SeguroExists(int id)
        {
            var Seguro = new List<Seguro>();

            var url = URLApi;

            // Envio da requisição a fim de autenticar
            // e obter o token de acesso
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile($"appsettings.json");
            var config = builder.Build();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage respToken = client.PostAsync(
                   URLApi + "Auth", new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            Email = config.GetSection("API_Access:UserID").Value,
                            PasswordHash = config.GetSection("API_Access:UserPWD").Value,
                            AccessKey = config.GetSection("API_Access:AccessKey").Value
                        }), Encoding.UTF8, "application/json")).Result;

                string conteudo =
                    respToken.Content.ReadAsStringAsync().Result;
                Console.WriteLine(conteudo);

                if (respToken.StatusCode == HttpStatusCode.OK)
                {
                    TokenID token = JsonConvert.DeserializeObject<TokenID>(conteudo);
                    if (token.Token != null)
                    {
                        client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", token.Token);

                        //Conteudo Inicio

                        var response = client.GetAsync(url + "Seguro/" + id).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var results = response.Content.ReadAsStringAsync().Result;

                            Seguro = JsonConvert.DeserializeObject<Seguro[]>(results).ToList();
                        }
                        if (Seguro == null)
                        {
                            return true;
                        }

                        //Conteudo Fim
                    }
                }

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
