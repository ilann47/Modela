using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modela.Data;
using Modela.Models;

namespace Modela.Controllers;
[Authorize]
public class EstadoController : Controller {

    private IEstadoRepository _estadoRepository;
    public EstadoController(IEstadoRepository estadoRepository) {
        _estadoRepository = estadoRepository;
    }

    public async Task<ActionResult> Index() {
        return View(await _estadoRepository.GetTodos());
    }

}
