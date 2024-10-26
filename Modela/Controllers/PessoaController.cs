using Microsoft.AspNetCore.Mvc;

namespace Modela.Controllers;
public class PessoaController : Controller {
    public IActionResult Index() {
        return View();
    }

    public IActionResult Edit(int id) {
        return View("Index");
    }

    public IActionResult Create() {
        return View("Index");
    }

    public IActionResult Update(int id) {
        return View("Index");
    }

    public IActionResult Delete(int id) {
        return View("Index");
    }

}
