using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Context;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class UsuarioRepositories : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepositories(AppDbContext context)
        {
            _context = context;
        }

        public void Alterar(Usuario usuario)
        {
            _context.Entry(usuario).State = EntityState.Modified;
        }

        public async Task<Usuario> AuthenticateAsync(string email, string senha)
        {
            return await _context.Usuarios.Where(x => x.Email == email).FirstOrDefaultAsync();
        }

        public void Excluir(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
        }

        public void Incluir(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Usuario> SelecionarByPk(int id)
        {
            return await _context.Usuarios.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Usuario> SelecionarEmail(string email, string passwordHash)
        {
            return await _context.Usuarios.Where(x => x.Email == email && x.PasswordHash == passwordHash).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Usuario>> SelecionarTodos()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario> UserExists(string email)
        {
            return await _context.Usuarios.Where(x => x.Email == email).FirstOrDefaultAsync();
        }
    }
}
