using Modela.Data.MySQLRepositories;
using Modela.Models;
using System.Text.Json;

namespace Modela.Data.IBGERepositories;

public class CidadeIBERepository : ICidadeRepository {

    private readonly HttpClient _httpClient;

    private IPaisRepository _paisRepository;
    private IEstadoRepository _estadoRepository;

    public CidadeIBERepository(IPaisRepository paisRepository, IEstadoRepository estadoRepository) {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://servicodados.ibge.gov.br");


        _paisRepository = paisRepository;
        _estadoRepository = estadoRepository;
    }

    public async Task<List<Cidade>> GetByEstadoId(int estadoId) {
        Estado? estado = (await _estadoRepository.GetTodos()).Find(e => e.Id == estadoId);

        if (estado == null) {
            return []; 
        }

        Stream responseStream = await _httpClient.GetStreamAsync($"/api/v1/localidades/estados/{estadoId}/municipios?orderBy=nome");

        List<Dictionary<string, object>>? apiResponse = await JsonSerializer.DeserializeAsync<List<Dictionary<string, object>>>(responseStream);

        List<Cidade> cidades = [];
        if (apiResponse != null) {
            foreach (Dictionary<string, object> item in apiResponse) {
                int id = int.Parse(item["id"].ToString() ?? "0");
                string nome = item["nome"].ToString() ?? string.Empty;

                cidades.Add(new Cidade(id, nome, estado));
            }
        }

        return cidades;
    }

    public Task<Cidade> GetById(int id) {
        throw new NotImplementedException();
    }

    public Task<Cidade> GetByNome(string name) {
        throw new NotImplementedException();
    }

    public Task<List<Cidade>> GetTodos() {
        throw new NotImplementedException();
    }

}
