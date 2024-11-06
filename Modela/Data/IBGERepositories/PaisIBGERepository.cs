using Modela.Data.MySQLRepositories;
using Modela.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Modela.Data.IBGERepositories;

public class PaisIBGERepository : IPaisRepository {
    
    private readonly HttpClient _httpClient;

    public PaisIBGERepository() {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://servicodados.ibge.gov.br");
    }

    public async Task<List<Pais>> GetTodos() {
        Stream responseStream = await _httpClient.GetStreamAsync("/api/v1/localidades/paises?orderBy=nome");

        List<Dictionary<string, object>>? apiResponse = await JsonSerializer.DeserializeAsync<List<Dictionary<string, object>>>(responseStream);

        List<Pais> paises = [];
        if (apiResponse != null) {
            foreach (var item in apiResponse) {
                JsonElement id = (JsonElement) item["id"];

                int m49 = id.GetProperty("M49").GetInt32();
                string isoAlpha3 = id.GetProperty("ISO-ALPHA-3").GetString() ?? "";
                string nome = item["nome"].ToString() ?? "";

                string nomeLimpo = Regex.Replace(nome, @"\s*\(.*?\)", "").Trim();

                paises.Add(new Pais(m49, nomeLimpo, isoAlpha3));
            }
        }

        return paises;
    }

    public async Task<Pais?> GetByName(string nome) {
        List<Pais> paises = await GetTodos();

        if (paises.Count == 0) return null;

        return paises.Find(p => p.Nome.StartsWith(nome, StringComparison.CurrentCultureIgnoreCase));
    }

}
