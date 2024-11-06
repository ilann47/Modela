using System.ComponentModel.DataAnnotations;

namespace Modela.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; } = 0;

        public string? Nome { get; set; }
        public string? CPF { get; set; }
        public string? RG { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Telefone { get; set; }
        public string? EstadoCivil { get; set; }
        public string? CEP { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Cidade { get; set; }

        public Cidade? OCidade { get; set; }

        public Cliente()
        {
            ClienteId = 0;
            Nome = "";
            CPF = "";
            RG = "";
            DataNascimento = DateTime.Now;
            Telefone = "";
            EstadoCivil = "";
            CEP = "";
            Logradouro = "";
            Numero = "";
            Complemento = "";
            Cidade = "";
        }

        public Cliente(
            int clienteId,
            string nome,
            string cpf,
            string rg,
            DateTime dataNascimento,
            string telefone,
            string estadoCivil,
            string cep,
            string logradouro,
            string numero,
            string complemento,
            string cidade,
            Cidade oCidade
        )
           
        {
            ClienteId = clienteId;
            Nome = nome;
            CPF = cpf;
            RG = rg;
            DataNascimento = dataNascimento;
            Telefone = telefone;
            EstadoCivil = estadoCivil;
            CEP = cep;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Cidade = cidade;
            OCidade = oCidade;
        }
    }
}
