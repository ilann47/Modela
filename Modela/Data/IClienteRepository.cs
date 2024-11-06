using Modela.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Modela.Data
{
    public interface IClienteRepository
    {
        Task<List<Cliente>> GetTodos();
        Task<Cliente?> GetById(int id);
        Task<List<Cliente>> GetByNome(string nome);
        Task Add(Cliente cliente);
        Task Update(Cliente cliente);  
        Task Delete(int id);           
    }
}
