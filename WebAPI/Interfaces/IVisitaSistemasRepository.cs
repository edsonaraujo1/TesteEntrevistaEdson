using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IVisitaSistemasRepository
    {
        void Incluir(VisitaSistemas visitaSistemas);
        void Alterar(VisitaSistemas visitaSistemas);
        void Excluir(VisitaSistemas visitaSistemas);
        Task<VisitaSistemas> SelecionarByPk(int id);
        Task<IEnumerable<VisitaSistemas>> SelecionarTodos();
        Task<bool> SaveAllAsync();
    }
}
