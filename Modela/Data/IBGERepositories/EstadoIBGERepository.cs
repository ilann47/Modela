using Modela.Models;
using System.Text.Json;

namespace Modela.Data.IBGERepositories;

public class EstadoIBGERepository : IEstadoRepository {
    
    private readonly HttpClient _httpClient;
    private IPaisRepository _paisRepository;

    public EstadoIBGERepository(IPaisRepository paisRepository) {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://servicodados.ibge.gov.br");

        _paisRepository = paisRepository;
    }

    public async Task<List<Estado>> GetTodos() {
        Stream responseStream = await _httpClient.GetStreamAsync("/api/v1/localidades/estados?orderBy=nome");

        List<Dictionary<string, object>>? apiResponse = await JsonSerializer.DeserializeAsync<List<Dictionary<string, object>>>(responseStream);

        List<Estado> estados = [];
        Pais paisBrail = await _paisRepository.GetByName("brasil") ?? new Pais();
        if (apiResponse != null) {
            foreach (Dictionary<string, object> item in apiResponse) {
                int id = int.Parse(item["id"].ToString() ?? "0");
                string sigla = item["sigla"].ToString() ?? string.Empty;
                string nome = item["nome"].ToString() ?? string.Empty;


                estados.Add(new Estado(id, nome, sigla, paisBrail));
            }
        }

        return estados;
    }

    public async Task<Estado?> GetByName(string nome) {
        List<Estado> paises = await GetTodos();

        if (paises.Count == 0) return null;

        return paises.Find(p => p.Nome.StartsWith(nome, StringComparison.CurrentCultureIgnoreCase));
    }

}
