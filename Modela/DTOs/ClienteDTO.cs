using Modela.Models;
using Modela.Services;
namespace Modela.DTOs
{
    public class ClienteDTO
    {
        public string? CPF { get; set; }
        public string? RG { get; set; }
        public string? Telefone { get; set; }
        public string? EstadoCivil { get; set; }
        public string? CEP { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Cidade { get; set; }
    }
}
