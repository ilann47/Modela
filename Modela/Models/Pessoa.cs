using System.ComponentModel.DataAnnotations;

namespace Modela.Models;

public class Pessoa {

    public int Id { get; set; } = 0;

    [Required]
    public string? Nome { get; set; }

    [Required]
    public string? Sobrenome { get; set; }

    public DateTime Nascimento { get; set; }

    public char Sexo { get; set; } = ' ';

    public Pessoa() { }

    public Pessoa(int id, string? nome, string? sobrenome, DateTime nascimento, char sexo) {
        Id = id;
        Nome = nome;
        Sobrenome = sobrenome;
        Nascimento = nascimento;
        Sexo = sexo;
    }

}
