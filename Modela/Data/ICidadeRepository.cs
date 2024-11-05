using Modela.Models;

namespace Modela.Data;

public interface ICidadeRepository {

    public Task<List<Cidade>> GetTodos();
    public Task<List<Cidade>> GetByEstadoId(int estadoId);
    public Task<Cidade> GetByNome(string name);
    public Task<Cidade> GetById(int id);

}
