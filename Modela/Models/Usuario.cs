using System.ComponentModel.DataAnnotations;

namespace Modela.Models;

public class Usuario {

    public Pessoa Pessoa { get; set; }

    public int Id { get; set; } = 0;

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }

    public DateTime PrimeiroLogin { get; set; }

    public Usuario() {
        Pessoa = new Pessoa();
    }

    public Usuario(
        int pessoaId,
        string nome,
        string sobrenome,
        DateTime nascimento,
        char sexo,
        int id,
        string? email,
        string? password,
        DateTime primeiroLogin
     ) {
        Id = id;
        Email = email;
        Password = password;
        PrimeiroLogin = primeiroLogin;

        Pessoa = new Pessoa(pessoaId, nome, sobrenome, nascimento, sexo);
    }

}
