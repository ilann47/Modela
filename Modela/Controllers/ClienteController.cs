using Microsoft.AspNetCore.Mvc;
using Modela.Data;
using Modela.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Modela.ViewModels;

namespace Modela.Controllers {
    [Route("Cliente")] // Define a rota base para o controlador
    public class ClienteController : Controller {
        private readonly IClienteRepository _clienteRepository;
        private readonly IEstadoRepository _estadoRepository;
        private readonly ICidadeRepository _cidadeRepository;

        public ClienteController(IClienteRepository clienteRepository, IEstadoRepository estadoRepository, ICidadeRepository cidadeRepository) {
            _clienteRepository = clienteRepository;
            _estadoRepository = estadoRepository;
            _cidadeRepository = cidadeRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index() {
            List<Cliente> clientes = await _clienteRepository.GetTodos();

            return View(clientes);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create(int selectedEstadoId = 0) {
            List<Estado> estados = await _estadoRepository.GetTodos();
            List<Cidade> cidades = new List<Cidade>();

            if (selectedEstadoId > 0) {
                cidades = await _cidadeRepository.GetByEstadoId(selectedEstadoId);
            }

            ViewData["Estados"] = estados;
            ViewData["Cidades"] = cidades;
            ViewData["SelectedEstadoId"] = selectedEstadoId;

            return View(new Cliente());
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Cliente cliente, int selectedEstadoId, string cidadeNome) {
            if (ModelState.IsValid) {
                if (selectedEstadoId > 0 && !string.IsNullOrEmpty(cidadeNome)) {
                    cliente.Cidade = cidadeNome;
                }

                await _clienteRepository.Add(cliente);
                return RedirectToAction("Index");
            }

            List<Estado> estados = await _estadoRepository.GetTodos();
            List<Cidade> cidades = new List<Cidade>();

            if (selectedEstadoId > 0) {
                cidades = await _cidadeRepository.GetByEstadoId(selectedEstadoId);
            }

            ViewData["Estados"] = estados;
            ViewData["Cidades"] = cidades;
            ViewData["SelectedEstadoId"] = selectedEstadoId;

            return View(cliente);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, int selectedEstadoId = 0) {
            Cliente? cliente = await _clienteRepository.GetById(id);
            if (cliente == null) return NotFound();

            List<Estado> estados = await _estadoRepository.GetTodos();
            List<Cidade> cidades = new List<Cidade>();

            ViewData["Estados"] = estados;
            ViewData["Cidades"] = cidades;
            ViewData["SelectedEstadoId"] = selectedEstadoId;

            return View(cliente);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, Cliente cliente, int selectedEstadoId, string cidadeNome, bool isSubmit) {
            Cliente? clienteAntigo = await _clienteRepository.GetById(id); 

            if (clienteAntigo == null) return NotFound();

            if (cidadeNome == null) {
                cliente.Cidade = clienteAntigo.Cidade != string.Empty ? clienteAntigo.Cidade : string.Empty;
            } else {
                cliente.Cidade = cidadeNome;
            }

            ModelState.Remove("cidadeNome");
            ModelState.Remove("selectedEstadoId");

            if (!ModelState.IsValid || !isSubmit) {
                List<Estado> estados = await _estadoRepository.GetTodos();
                List<Cidade> cidades = await _cidadeRepository.GetByEstadoId(selectedEstadoId);

                ViewData["Estados"] = estados;
                ViewData["Cidades"] = cidades;
                ViewData["SelectedEstadoId"] = selectedEstadoId;

                return View(cliente);
            }

            cliente.ClienteId = id;
            await _clienteRepository.Update(cliente);

            return RedirectToAction("Index");
        }


        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id) {
            Cliente? cliente = await _clienteRepository.GetById(id);
            if (cliente == null) {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id) {
            Cliente? cliente = await _clienteRepository.GetById(id);
            if (cliente == null) {
                return NotFound();
            }

            await _clienteRepository.Delete(cliente.ClienteId);
            return RedirectToAction("Index");
        }
    }
}
