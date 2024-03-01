using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IContatoEmailRepository
    {
        void Incluir(ContatoEmail contatoEmail);
        void Alterar(ContatoEmail contatoEmail);
        void Excluir(ContatoEmail contatoEmail);
        Task<ContatoEmail> SelecionarByPk(int id);
        Task<IEnumerable<ContatoEmail>> SelecionarTodos();
        Task<bool> SaveAllAsync();
    }
}
