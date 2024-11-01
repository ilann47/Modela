using Modela.Models;
using Modela.DTOs;
using System.Collections.Generic;

namespace Modela.Services
{
    public interface IClienteService
    {
        Cliente Create(ClienteDTO clienteDTO);
        Cliente GetById(int id);
        IEnumerable<Cliente> GetAll();
        Cliente Update(int id, ClienteDTO clienteDTO);
    }
}