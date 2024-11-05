namespace Modela.Models;

public class Estado {

    public int Id { get; set; }
    public string Nome { get; set; }
    public string Sigla { get; set; }

    public Pais Pais { get; set; }

    public Estado(int id, string nome, string sigla, Pais pais) {
        Id = id;
        Nome = nome;
        Sigla = sigla;
        Pais = pais;
    }

    public Estado() {
        Nome = "";
        Sigla = "";
        Pais = new Pais();
    }

}
