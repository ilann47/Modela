using System;
using System.ComponentModel.DataAnnotations;

namespace Modela.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        public string CPF { get; set; } = string.Empty;
        public string RG { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; } = DateTime.MinValue;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public char Sexo { get; set; } = ' ';
        public string EstadoCivil { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty; 
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
    }
}
