using Modela.Models;

namespace Modela.Data;

public interface IEstadoRepository {

    public Task<List<Estado>> GetTodos();
    public Task<Estado?> GetByName(string nome);

}
