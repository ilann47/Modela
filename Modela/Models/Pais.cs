namespace Modela.Models;

public class Pais {

    public int Id { get; set; }
    public string Nome { get; set; }

    public string Sigla { get; set; }

    public Pais(int id, string nome, string sigla) {
        Id = id;
        Nome = nome;
        Sigla = sigla;
    }

    public Pais() {
        Nome = "";
        Sigla = "";
    }
}
