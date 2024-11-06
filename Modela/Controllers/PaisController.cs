using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modela.Data;
using Modela.Data.MySQLRepositories;
using Modela.Models;

namespace Modela.Controllers;
[Authorize]
public class PaisController : Controller {

    private IPaisRepository _paisRepository;
    public PaisController(IPaisRepository paisRepository) {
        _paisRepository = paisRepository;
    }

    public async Task<ActionResult> Index() {
        return View(await _paisRepository.GetTodos());
    }

}
