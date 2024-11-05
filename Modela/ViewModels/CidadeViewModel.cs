using Modela.Models;

namespace Modela.ViewModels;

public class CidadeViewModel {

    public List<Estado> Estados { get; set; }
    public List<Cidade> Cidades { get; set; }
    public Estado? SelectedEstado { get; set; }


    public CidadeViewModel(List<Estado> estados, List<Cidade> cidades, Estado? selectedEstado = null) {
        Estados = estados;
        Cidades = cidades;
        SelectedEstado = selectedEstado;
    }

}
