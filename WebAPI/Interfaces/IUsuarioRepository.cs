using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IUsuarioRepository
    {
        void Incluir(Usuario seguro);
        void Alterar(Usuario seguro);
        void Excluir(Usuario seguro);
        Task<Usuario> SelecionarByPk(int id);
        Task<Usuario> SelecionarEmail(string email, string passwordHash);
        Task<IEnumerable<Usuario>> SelecionarTodos();
        Task<bool> SaveAllAsync();
        Task<Usuario> AuthenticateAsync(string email, string senha);
        Task<Usuario> UserExists(string email);
        
    }
}
