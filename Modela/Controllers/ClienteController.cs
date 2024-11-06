using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modela.Data;
using Modela.Models;
using System.Threading.Tasks;

namespace Modela.Controllers
{
    [Authorize]
    public class ClienteController : Controller
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly ICidadeRepository _cidadeRepository;

        public ClienteController(IClienteRepository clienteRepository, ICidadeRepository cidadeRepository)
        {
            _clienteRepository = clienteRepository;
            _cidadeRepository = cidadeRepository;
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteRepository.GetTodos();
            return View(clientes);
        }

        public async Task<IActionResult> Create()
        {
            var cidades = await _cidadeRepository.GetTodos();
            ViewBag.Cidades = cidades;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                await _clienteRepository.Add(cliente);
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _clienteRepository.GetById(id);
            if (cliente == null) return NotFound();

            ViewBag.Cidades = await _cidadeRepository.GetTodos();
            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                await _clienteRepository.Update(cliente);
                return RedirectToAction("Index");
            }
            ViewBag.Cidades = await _cidadeRepository.GetTodos();
            return View(cliente);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _clienteRepository.GetById(id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clienteRepository.Delete(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Search(string nome)
        {
            var clientes = await _clienteRepository.GetByNome(nome);
            return View("Index", clientes);
        }
    }
}
