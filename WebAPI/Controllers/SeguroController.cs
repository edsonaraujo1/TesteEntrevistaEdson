using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Context;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguroController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SeguroController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Seguro
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seguro>>> GetSeguros()
        {
            return await _context.Seguros.ToListAsync();
        }

        // GET: api/Seguro/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Seguro>> GetSeguro(int id)
        {
            var seguro = await _context.Seguros.FindAsync(id);

            if (seguro == null)
            {
                return NotFound();
            }

            return seguro;
        }

        // PUT: api/Seguro/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeguro(int id, Seguro seguro)
        {
            if (id != seguro.id)
            {
                return BadRequest();
            }

            CalculaValorSeguro calculo = new CalculaValorSeguro();
            double com_pre = calculo.CalculaSeguro(seguro.ValorVeiculo);
            seguro.ValorSeguro = com_pre;

            _context.Entry(seguro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeguroExists(id))
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

        // POST: api/Seguro
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Seguro>> PostSeguro(Seguro seguro)
        {
            CalculaValorSeguro calculo = new CalculaValorSeguro();
            double com_pre = calculo.CalculaSeguro(seguro.ValorVeiculo);
            seguro.ValorSeguro = com_pre;

            _context.Seguros.Add(seguro);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSeguro", new { id = seguro.id }, seguro);
        }

        // DELETE: api/Seguro/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeguro(int id)
        {
            var seguro = await _context.Seguros.FindAsync(id);
            if (seguro == null)
            {
                return NotFound();
            }

            _context.Seguros.Remove(seguro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SeguroExists(int id)
        {
            return _context.Seguros.Any(e => e.id == id);
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
