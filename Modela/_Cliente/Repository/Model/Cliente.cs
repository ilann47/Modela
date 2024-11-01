using System.ComponentModel.DataAnnotations;
using Modela._Home.Repository.Model;

namespace Modela._Cliente.Repository.Model
{
    public class Cliente : Usuario
    {
        public Cliente oCliente;
        public int ClienteId { get; set; } = 0;

        public string? CPF { get; set; }
        public string? RG { get; set; }
        public string? Telefone { get; set; }
        public string? EstadoCivil { get; set; }
        public string? CEP { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Cidade { get; set; }

        public Cliente()
        {
            ClienteId = 0;
            CPF = "";
            RG = "";
            Telefone = "";
            EstadoCivil = "";
            CEP = "";
            Logradouro = "";
            Numero = "";
            Complemento = "";
            Cidade = "";
            oCliente = new Cliente();
        }

        public Cliente(
            int pessoaId,
            string nome,
            string sobrenome,
            DateTime nascimento,
            char sexo,
            int id,
            string? email,
            string? password,
            DateTime primeiroLogin,
            int clienteId,
            string cpf,
            string rg,
            string telefone,
            string estadoCivil,
            string cep,
            string logradouro,
            string numero,
            string complemento,
            string cidade,
            Cliente oCliente
        )
            : base(
                pessoaId,
                nome,
                sobrenome,
                nascimento,
                sexo,
                id,
                email,
                password,
                primeiroLogin
            )
        {
            ClienteId = id;
            CPF = cpf;
            RG = rg;
            Telefone = telefone;
            EstadoCivil = estadoCivil;
            CEP = cep;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Cidade = cidade;
            this.oCliente = oCliente;
        }
    }
}
