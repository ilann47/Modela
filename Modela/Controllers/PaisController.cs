using Microsoft.AspNetCore.Mvc;
using Modela.Data;
using Modela.Models;

namespace Modela.Controllers;
public class PaisController : Controller {

    private IPaisRepository _paisRepository;
    public PaisController(IPaisRepository paisRepository) {
        _paisRepository = paisRepository;
    }

    public async Task<ActionResult> Index() {
        return View(await _paisRepository.GetTodos());
    }

}
