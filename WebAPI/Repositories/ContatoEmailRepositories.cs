using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Context;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class ContatoEmailRepositories : IContatoEmailRepository
    {
        private readonly AppDbContext _context;

        public ContatoEmailRepositories(AppDbContext context)
        {
            _context = context;
        }
        public void Alterar(ContatoEmail contatoEmail)
        {
            _context.Entry(contatoEmail).State = EntityState.Modified;
        }
        public void Excluir(ContatoEmail contatoEmail)
        {
            _context.ContatoEmails.Remove(contatoEmail);
        }
        public void Incluir(ContatoEmail contatoEmail)
        {
            _context.ContatoEmails.Add(contatoEmail);
        }
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<ContatoEmail> SelecionarByPk(int id)
        {
            return await _context.ContatoEmails.Where(x => x.id == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<ContatoEmail>> SelecionarTodos()
        {
            return await _context.ContatoEmails.ToListAsync();
        }
    }
}
