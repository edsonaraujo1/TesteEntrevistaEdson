using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Interfaces;
using WebApi.Models;
using WebAPI.Context;

namespace WebApi.Repositories
{
    public class SeguroRepositories : ISeguroRepository
    {
        private readonly AppDbContext _context;
        
        public SeguroRepositories(AppDbContext context)
        {
            _context = context;
        }
        public void Alterar(Seguro seguro)
        {
            _context.Entry(seguro).State = EntityState.Modified;
        }
        public void Excluir(Seguro seguro)
        {
            _context.Seguros.Remove(seguro);
        }
        public void Incluir(Seguro seguro)
        {
            _context.Seguros.Add(seguro);
        }
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<Seguro> SelecionarByPk(int id)
        {
            return await _context.Seguros.Where(x => x.id == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Seguro>> SelecionarTodos()
        {
            return await _context.Seguros.ToListAsync();
        }
    }
}
