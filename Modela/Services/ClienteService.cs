using Modela.Models;
using Modela.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace Modela.Services
{
    public class ClienteService : IClienteService
    {
        // Lista em memória para simular o banco de dados
        private readonly List<Cliente> _clientes = new();
        private int _nextId = 1;

        // Cria um novo cliente
        public Cliente Create(ClienteDTO clienteDTO)
        {
            var cliente = new Cliente
            {
                ClienteId = _nextId++,
                CPF = clienteDTO.CPF,
                RG = clienteDTO.RG,
                Telefone = clienteDTO.Telefone,
                EstadoCivil = clienteDTO.EstadoCivil,
                CEP = clienteDTO.CEP,
                Logradouro = clienteDTO.Logradouro,
                Numero = clienteDTO.Numero,
                Complemento = clienteDTO.Complemento,
                Cidade = clienteDTO.Cidade
            };

            _clientes.Add(cliente);
            return cliente;
        }

        // Obtém um cliente por ID
        public Cliente GetById(int id)
        {
            return _clientes.FirstOrDefault(c => c.ClienteId == id);
        }

        // Obtém todos os clientes
        public IEnumerable<Cliente> GetAll()
        {
            return _clientes;
        }

        // Atualiza um cliente existente
        public Cliente Update(int id, ClienteDTO clienteDTO)
        {
            var cliente = GetById(id);
            if (cliente == null)
            {
                return null;
            }

            cliente.CPF = clienteDTO.CPF;
            cliente.RG = clienteDTO.RG;
            cliente.Telefone = clienteDTO.Telefone;
            cliente.EstadoCivil = clienteDTO.EstadoCivil;
            cliente.CEP = clienteDTO.CEP;
            cliente.Logradouro = clienteDTO.Logradouro;
            cliente.Numero = clienteDTO.Numero;
            cliente.Complemento = clienteDTO.Complemento;
            cliente.Cidade = clienteDTO.Cidade;

            return cliente;
        }
    }
}
