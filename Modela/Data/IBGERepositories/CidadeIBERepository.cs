// Modela/Data/IBGERepositories/CidadeIBERepository.cs
using Modela.Models;
using System.Text.Json;

namespace Modela.Data.IBGERepositories
{
    public class CidadeIBERepository : ICidadeRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IPaisRepository _paisRepository;
        private readonly IEstadoRepository _estadoRepository;

        public CidadeIBERepository(IPaisRepository paisRepository, IEstadoRepository estadoRepository)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://servicodados.ibge.gov.br") };
            _paisRepository = paisRepository;
            _estadoRepository = estadoRepository;
        }

        public async Task<List<Cidade>> GetByEstadoId(int estadoId)
        {
            Estado? estado = (await _estadoRepository.GetTodos()).Find(e => e.Id == estadoId);
            if (estado == null) return new List<Cidade>();

            var responseStream = await _httpClient.GetStreamAsync($"/api/v1/localidades/estados/{estadoId}/municipios?orderBy=nome");
            var apiResponse = await JsonSerializer.DeserializeAsync<List<Dictionary<string, object>>>(responseStream);

            var cidades = new List<Cidade>();
            if (apiResponse != null)
            {
                foreach (var item in apiResponse)
                {
                    int id = int.Parse(item["id"].ToString() ?? "0");
                    string nome = item["nome"].ToString() ?? string.Empty;
                    cidades.Add(new Cidade(id, nome, estado));
                }
            }

            return cidades;
        }

        public async Task<List<Cidade>> GetTodos()
        {
            // Implementa a busca de todas as cidades de todos os estados.
            var estados = await _estadoRepository.GetTodos();
            var cidades = new List<Cidade>();

            foreach (var estado in estados)
            {
                var cidadesDoEstado = await GetByEstadoId(estado.Id);
                cidades.AddRange(cidadesDoEstado);
            }

            return cidades;
        }

        public async Task<Cidade> GetById(int id)
        {
            var estados = await _estadoRepository.GetTodos();

            foreach (var estado in estados)
            {
                var cidadesDoEstado = await GetByEstadoId(estado.Id);
                var cidade = cidadesDoEstado.FirstOrDefault(c => c.Id == id);

                if (cidade != null)
                {
                    return cidade;
                }
            }

            // Retorne null ou uma cidade padrão
            return null;  // Ou lançar uma exceção personalizada
        }



        public Task<Cidade> GetByNome(string name)
        {
            throw new NotImplementedException();
        }
    }
}
