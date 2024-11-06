using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modela.Data;
using Modela.Models;
using Modela.ViewModels;

namespace Modela.Controllers;
[Authorize]
public class CidadeController : Controller {

    private readonly IEstadoRepository _estadoRepository;
    private readonly ICidadeRepository _cidadeRepository;

    public CidadeController(IEstadoRepository estadoRepository, ICidadeRepository cidadeRepository) {
        _estadoRepository = estadoRepository;
        _cidadeRepository = cidadeRepository;
    }

    [Route("cidade/{estadoId:int?}")]
    public async Task<IActionResult> Index(int estadoId = 0) {
        List<Estado> estados = await _estadoRepository.GetTodos();
        CidadeViewModel cidadeViewModel = new CidadeViewModel(estados, []);

        if (estadoId == 0) {
            return View(cidadeViewModel);
        }

        Estado? estado = estados.Find(e => e.Id == estadoId);

        if (estado == null) {
            return View(cidadeViewModel);
        }

        List<Cidade> cidades = await _cidadeRepository.GetByEstadoId(estado.Id);
        cidadeViewModel.Cidades = cidades;
        cidadeViewModel.SelectedEstado = estado;

        return View(cidadeViewModel);
    }


}
