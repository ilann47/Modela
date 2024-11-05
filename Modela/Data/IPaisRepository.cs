using Modela.Models;

namespace Modela.Data;

public interface IPaisRepository {

    public Task<List<Pais>> GetTodos();
    public Task<Pais?> GetByName(string nome);

}
