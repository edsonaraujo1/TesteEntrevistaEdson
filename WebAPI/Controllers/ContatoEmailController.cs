using System.Collections.Generic;
using System.Linq;
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
    public class ContatoEmailController : Controller
    {
        private readonly IContatoEmailRepository _contatoEmailRepositories;
        private readonly AppDbContext _context;

        public ContatoEmailController(AppDbContext context, IContatoEmailRepository contatoEmailRepositories)
        {
            _context = context;
            _contatoEmailRepositories = contatoEmailRepositories;
        }

        // GET: api/ContatoEmail
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ContatoEmail>>> GetContatoEmails()
        {
            return await _context.ContatoEmails.ToListAsync();
        }

        // GET: api/ContatoEmail/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ContatoEmail>> GetContatoEmail(int id)
        {
            var contatoEmail = await _context.ContatoEmails.FindAsync(id);

            if (contatoEmail == null)
            {
                return NotFound();
            }

            return contatoEmail;
        }

        // PUT: api/ContatoEmail/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutContatoEmail(int id, ContatoEmail contatoEmail)
        {
            if (id != contatoEmail.id)
            {
                return BadRequest();
            }

            _context.Entry(contatoEmail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContatoEmailExists(id))
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

        // POST: api/ContatoEmail
        [HttpPost]
        public async Task<ActionResult<ContatoEmail>> PostContatoEmail(ContatoEmail contatoEmail)
        {
            if (ModelState.IsValid)
            {
                contatoEmail.data = System.DateTime.Now;
                _contatoEmailRepositories.Incluir(contatoEmail);
                if (await _contatoEmailRepositories.SaveAllAsync())
                {
                    return Ok("Registro Salvo com Sucesso!");
                }
                
            }
            return BadRequest("Ocorreu um erro ao salvar registro.");
        }

        // DELETE: api/ContatoEmail/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<ContatoEmail>> DeleteContatoEmail(int id)
        {
            var contatoEmail = await _context.ContatoEmails.FindAsync(id);
            if (contatoEmail == null)
            {
                return NotFound();
            }

            _context.ContatoEmails.Remove(contatoEmail);
            await _context.SaveChangesAsync();

            return contatoEmail;
        }

        private bool ContatoEmailExists(int id)
        {
            return _context.ContatoEmails.Any(e => e.id == id);
        }
    }
}
