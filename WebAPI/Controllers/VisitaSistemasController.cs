using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Context;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitaSistemasController : Controller
    {
        private readonly IVisitaSistemasRepository _visitaSistemasRepositories;
        private readonly AppDbContext _context;

        public VisitaSistemasController(AppDbContext context, IVisitaSistemasRepository visitaSistemasRepositories)
        {
            _context = context;
            _visitaSistemasRepositories = visitaSistemasRepositories;
        }

        // GET: api/VisitaSistemas
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<VisitaSistemas>>> GetVisitaSistemas()
        {
            return await _context.VisitaSistemas.ToListAsync();
        }

        // GET: api/VisitaSistemas/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<VisitaSistemas>> GetVisitaSistemas(int id)
        {
            var visitaSistemas = await _context.VisitaSistemas.FindAsync(id);

            if (visitaSistemas == null)
            {
                return NotFound();
            }

            return visitaSistemas;
        }

        // PUT: api/VisitaSistemas/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutVisitaSistemas(int id, VisitaSistemas visitaSistemas)
        {
            if (id != visitaSistemas.id)
            {
                return BadRequest();
            }

            _context.Entry(visitaSistemas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisitaSistemasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/VisitaSistemas
        [HttpPost]
        public async Task<ActionResult<VisitaSistemas>> PostVisitaSistemas(VisitaSistemas visitaSistemas)
        {
            try
            {
                visitaSistemas.data = System.DateTime.Now;
                // Obtem o IP Real do Usuario
                string Informa = "";
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
                foreach (IPAddress addr in localIPs)
                {
                    if (addr.AddressFamily.ToString() != "InterNetworkV6")
                    {
                        Informa = addr.ToString();
                    }
                }
                visitaSistemas.ip = Informa;
                _visitaSistemasRepositories.Incluir(visitaSistemas);
                if (await _visitaSistemasRepositories.SaveAllAsync())
                {
                    return Ok("Registro Salvo com Sucesso!");
                }

                return BadRequest("Ocorreu um erro ao salvar registro.");
            }
            catch (System.Exception e)
            {

                return Ok("Ocorreu Um Erro: " + e.Message.ToString());
            }

            
        }

        // DELETE: api/VisitaSistemas/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<VisitaSistemas>> DeleteVisitaSistemas(int id)
        {
            var visitaSistemas = await _context.VisitaSistemas.FindAsync(id);
            if (visitaSistemas == null)
            {
                return NotFound();
            }

            _context.VisitaSistemas.Remove(visitaSistemas);
            await _context.SaveChangesAsync();

            return visitaSistemas;
        }

        private bool VisitaSistemasExists(int id)
        {
            return _context.VisitaSistemas.Any(e => e.id == id);
        }
    }
}
