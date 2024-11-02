// ClienteController.cs
using Microsoft.AspNetCore.Mvc;
using Modela.Models;
using System.Collections.Generic;
using Modela.Services;
using Modela.DTOs;

namespace Modela.Controllers
{
    public class ClienteController : HomeController
    {
        private readonly IClienteService _clienteService;

        public ClienteController(ILogger<HomeController> logger, IClienteService clienteService)
            : base(logger)
        {
            _clienteService = clienteService;
        }

        // GET: api/Cliente/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(); // Renderiza a view Create.cshtml para exibir o formulário
        }

        // POST: api/Cliente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ClienteDTO clienteDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados de cliente inválidos na tentativa de criação.");
                return View(clienteDTO); // Renderiza a view com os erros de validação
            }

            var cliente = _clienteService.Create(clienteDTO);
            return RedirectToAction("GetById", new { id = cliente.ClienteId });
        }

        // GET: api/Cliente/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var cliente = _clienteService.GetById(id);
            if (cliente == null)
            {
                _logger.LogWarning($"Cliente com ID {id} não encontrado.");
                return NotFound("Cliente não encontrado");
            }

            return Ok(cliente);
        }

        // GET: api/Cliente
        [HttpGet]
        public IActionResult GetAll()
        {
            var clientes = _clienteService.GetAll();
            return Ok(clientes);
        }

        // PUT: api/Cliente/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ClienteDTO clienteDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Dados de cliente inválidos na tentativa de atualização.");
                return BadRequest(ModelState);
            }

            var clienteAtualizado = _clienteService.Update(id, clienteDTO);
            if (clienteAtualizado == null)
            {
                _logger.LogWarning($"Cliente com ID {id} não encontrado para atualização.");
                return NotFound("Cliente não encontrado");
            }

            return Ok(clienteAtualizado);
        }

        // Método de erro herdado do controlador pai
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public new IActionResult Error()
        {
            _logger.LogError("Erro no ClienteController.");
            return base.Error();
        }
    }
}
