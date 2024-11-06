using Modela.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Modela.Data.MySQLRepositories
{
    [Authorize]
    public class ClienteMySQLRepository : IClienteRepository
    {
        private readonly string _connectionString;

        public ClienteMySQLRepository(string connectionString) // Recebe a string de conexão via DI
        {
            _connectionString = connectionString;
        }

        public async Task Add(Cliente cliente)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            var query = "INSERT INTO clientes (Nome, CPF, RG, DataNascimento, Telefone, EstadoCivil, CEP, Logradouro, Numero, Complemento, Cidade) VALUES (@Nome, @CPF, @RG, @DataNascimento, @Telefone, @EstadoCivil, @CEP, @Logradouro, @Numero, @Complemento, @Cidade)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Nome", cliente.Nome);
            command.Parameters.AddWithValue("@CPF", cliente.CPF);
            command.Parameters.AddWithValue("@RG", cliente.RG);
            command.Parameters.AddWithValue("@DataNascimento", cliente.DataNascimento);
            command.Parameters.AddWithValue("@Telefone", cliente.Telefone);
            command.Parameters.AddWithValue("@EstadoCivil", cliente.EstadoCivil);
            command.Parameters.AddWithValue("@CEP", cliente.CEP);
            command.Parameters.AddWithValue("@Logradouro", cliente.Logradouro);
            command.Parameters.AddWithValue("@Numero", cliente.Numero);
            command.Parameters.AddWithValue("@Complemento", cliente.Complemento);
            command.Parameters.AddWithValue("@Cidade", cliente.Cidade);

            await command.ExecuteNonQueryAsync();
        }

        public async Task Update(Cliente cliente)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();


            var dataNascimento = cliente.DataNascimento.HasValue ? cliente.DataNascimento.Value.ToString("yyyy-MM-dd HH:mm:ss") : null;

            var query = "UPDATE clientes SET Nome=@Nome, CPF=@CPF, RG=@RG, DataNascimento=@DataNascimento, Telefone=@Telefone, EstadoCivil=@EstadoCivil, CEP=@CEP, Logradouro=@Logradouro, Numero=@Numero, Complemento=@Complemento, Cidade=@Cidade WHERE ClienteId=@ClienteId";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ClienteId", cliente.ClienteId);
            command.Parameters.AddWithValue("@Nome", cliente.Nome);
            command.Parameters.AddWithValue("@CPF", cliente.CPF);
            command.Parameters.AddWithValue("@RG", cliente.RG);
            command.Parameters.AddWithValue("@DataNascimento", (object)dataNascimento ?? DBNull.Value);
            command.Parameters.AddWithValue("@Telefone", cliente.Telefone);
            command.Parameters.AddWithValue("@EstadoCivil", cliente.EstadoCivil);
            command.Parameters.AddWithValue("@CEP", cliente.CEP);
            command.Parameters.AddWithValue("@Logradouro", cliente.Logradouro);
            command.Parameters.AddWithValue("@Numero", cliente.Numero);
            command.Parameters.AddWithValue("@Complemento", cliente.Complemento);
            command.Parameters.AddWithValue("@Cidade", cliente.Cidade);

            await command.ExecuteNonQueryAsync();
        }


        public async Task Delete(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            var query = "DELETE FROM clientes WHERE ClienteId=@ClienteId";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ClienteId", id);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<Cliente>> GetTodos()
        {
            List<Cliente> clientes = new();
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            var query = "SELECT * FROM clientes";

            using var command = new MySqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                // Verifique se o campo DataNascimento é nulo
                DateTime? dataNascimento = reader.IsDBNull(reader.GetOrdinal("DataNascimento"))
                    ? (DateTime?)null
                    : reader.GetDateTime("DataNascimento");

                clientes.Add(new Cliente
                {
                    ClienteId = reader.GetInt32("ClienteId"),
                    Nome = reader.GetString("Nome"),
                    CPF = reader.GetString("CPF"),
                    RG = reader.GetString("RG"),
                    DataNascimento = dataNascimento,  
                    Telefone = reader.GetString("Telefone"),
                    EstadoCivil = reader.GetString("EstadoCivil"),
                    CEP = reader.GetString("CEP"),
                    Logradouro = reader.GetString("Logradouro"),
                    Numero = reader.GetString("Numero"),
                    Complemento = reader.GetString("Complemento"),
                    Cidade = reader.GetString("Cidade")
                });
            }
            return clientes;
        }


        public async Task<Cliente?> GetById(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            var query = "SELECT * FROM clientes WHERE ClienteId = @id";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Cliente
                {
                    ClienteId = reader.GetInt32("ClienteId"),
                    Nome = reader.GetString("Nome"),
                    CPF = reader.GetString("CPF"),
                    RG = reader.GetString("RG"),
                    DataNascimento = reader.GetDateTime("DataNascimento"),
                    Telefone = reader.GetString("Telefone"),
                    EstadoCivil = reader.GetString("EstadoCivil"),
                    CEP = reader.GetString("CEP"),
                    Logradouro = reader.GetString("Logradouro"),
                    Numero = reader.GetString("Numero"),
                    Complemento = reader.GetString("Complemento"),
                    Cidade = reader.GetString("Cidade")
                };
            }
            return null;
        }

        public async Task<List<Cliente>> GetByNome(string nome)
        {
            List<Cliente> clientes = new();
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            var query = "SELECT * FROM clientes WHERE Nome LIKE @nome";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@nome", $"%{nome}%");
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                clientes.Add(new Cliente
                {
                    ClienteId = reader.GetInt32("ClienteId"),
                    Nome = reader.GetString("Nome"),
                    CPF = reader.GetString("CPF"),
                    RG = reader.GetString("RG"),
                    DataNascimento = reader.GetDateTime("DataNascimento"),
                    Telefone = reader.GetString("Telefone"),
                    EstadoCivil = reader.GetString("EstadoCivil"),
                    CEP = reader.GetString("CEP"),
                    Logradouro = reader.GetString("Logradouro"),
                    Numero = reader.GetString("Numero"),
                    Complemento = reader.GetString("Complemento"),
                    Cidade = reader.GetString("Cidade")
                });
            }
            return clientes;
        }
    }
}
