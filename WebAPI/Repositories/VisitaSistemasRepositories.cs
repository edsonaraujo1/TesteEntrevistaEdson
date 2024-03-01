using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Context;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class VisitaSistemasRepositories : IVisitaSistemasRepository
    {
        private readonly AppDbContext _context;

        public VisitaSistemasRepositories(AppDbContext context)
        {
            _context = context;
        }
        public void Alterar(VisitaSistemas visitaSistemas)
        {
            _context.Entry(visitaSistemas).State = EntityState.Modified;
        }
        public void Excluir(VisitaSistemas visitaSistemas)
        {
            _context.VisitaSistemas.Remove(visitaSistemas);
        }
        public void Incluir(VisitaSistemas visitaSistemas)
        {
            _context.VisitaSistemas.Add(visitaSistemas);
        }
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<VisitaSistemas> SelecionarByPk(int id)
        {
            return await _context.VisitaSistemas.Where(x => x.id == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<VisitaSistemas>> SelecionarTodos()
        {
            return await _context.VisitaSistemas.ToListAsync();
        }
    }
}
