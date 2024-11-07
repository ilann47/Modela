using Microsoft.AspNetCore.Mvc;
using Modela.Data;
using Modela.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Modela.Controllers
{
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

            // Carrega o nome da cidade para cada cliente usando o CidadeId
            foreach (var cliente in clientes)
            {
                if (cliente.CidadeId != 0)
                {
                    cliente.OCidade = await _cidadeRepository.GetById(cliente.CidadeId);
                }
            }

            return View(clientes);
        }


        public async Task<IActionResult> Create()
        {
            // Carrega todas as cidades para exibição no dropdown
            ViewBag.Cidades = await _cidadeRepository.GetTodos();
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

            ViewBag.Cidades = await _cidadeRepository.GetTodos();
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
    }
}
