using Microsoft.AspNetCore.Mvc;
using Modela.Data;
using Modela.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Modela.Controllers
{
    [Route("Cliente")] // Define a rota base para o controlador
    public class ClienteController : Controller
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly ICidadeRepository _cidadeRepository;

        public ClienteController(IClienteRepository clienteRepository, ICidadeRepository cidadeRepository)
        {
            _clienteRepository = clienteRepository;
            _cidadeRepository = cidadeRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteRepository.GetTodos();
            foreach (var cliente in clientes)
            {
                if (cliente.CidadeId != 0)
                {
                    cliente.OCidade = await _cidadeRepository.GetById(cliente.CidadeId);
                }
            }
            return View(clientes);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Cidades = await _cidadeRepository.GetTodos();
            return View();
        }

        [HttpPost("Create")]
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

        [HttpGet("Edit/{id:int}")] // Rota para editar um cliente com ID específico
        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _clienteRepository.GetById(id);
            if (cliente == null) return NotFound();

            ViewBag.Cidades = await _cidadeRepository.GetTodos();
            return View(cliente);
        }

        [HttpPost("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                var existingCliente = await _clienteRepository.GetById(id);
                if (existingCliente == null) return NotFound();

                // Atualize todos os campos, incluindo CidadeId
                existingCliente.Nome = cliente.Nome;
                existingCliente.CPF = cliente.CPF;
                existingCliente.RG = cliente.RG;
                existingCliente.DataNascimento = cliente.DataNascimento;
                existingCliente.Telefone = cliente.Telefone;
                existingCliente.EstadoCivil = cliente.EstadoCivil;
                existingCliente.CEP = cliente.CEP;
                existingCliente.Logradouro = cliente.Logradouro;
                existingCliente.Numero = cliente.Numero;
                existingCliente.Complemento = cliente.Complemento;
                existingCliente.CidadeId = cliente.CidadeId; // Atualize o CidadeId

                await _clienteRepository.Update(existingCliente);

                return RedirectToAction("Index", "Cliente");
            }

            ViewBag.Cidades = await _cidadeRepository.GetTodos();
            return View(cliente);
        }


        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _clienteRepository.GetById(id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        [HttpPost("Delete/{id:int}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _clienteRepository.GetById(id);
            if (cliente == null) return NotFound();

            await _clienteRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
