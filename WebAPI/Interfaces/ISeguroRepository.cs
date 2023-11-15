using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface ISeguroRepository
    {
        void Incluir(Seguro seguro);
        void Alterar(Seguro seguro);
        void Excluir(Seguro seguro);
        Task<Seguro> SelecionarByPk(int id);
        Task<IEnumerable<Seguro>> SelecionarTodos();
        Task<bool> SaveAllAsync();
    }
}
