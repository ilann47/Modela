using System.Collections.Generic;
using Modela._Cliente.Repository.Dto;
using Modela._Cliente.Repository.Model;

namespace Modela._Cliente.Service
{
    public interface IClienteService
    {
        Cliente Create(ClienteDTO clienteDTO);
        Cliente GetById(int id);
        IEnumerable<Cliente> GetAll();
        Cliente Update(int id, ClienteDTO clienteDTO);
    }
}