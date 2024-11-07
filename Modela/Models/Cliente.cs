using System;
using System.ComponentModel.DataAnnotations;

namespace Modela.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }

        [Required]
        public string? Nome { get; set; }

        public string? CPF { get; set; }
        public string? RG { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public char? Sexo { get; set; }
        public string? EstadoCivil { get; set; }
        public string? CEP { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public int CidadeId { get; set; }
        public Cidade? OCidade { get; set; }
    }
}
