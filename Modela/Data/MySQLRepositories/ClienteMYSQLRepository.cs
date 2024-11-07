using Modela.Data.IBGERepositories;
using Modela.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Modela.Data.MySQLRepositories
{
    public class ClienteMySQLRepository : IClienteRepository
    {
        private readonly string _connectionString;
        private readonly ICidadeRepository _cidadeRepository;

        public ClienteMySQLRepository(string connectionString, ICidadeRepository cidadeRepository)
        {
            _connectionString = connectionString;
            _cidadeRepository = cidadeRepository;
        }

        public async Task Add(Cliente cliente)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                var query = @"
                    INSERT INTO clientes (Nome, CPF, RG, DataNascimento, Telefone, Email, Sexo, EstadoCivil, CEP, Logradouro, Numero, Complemento, CidadeId)
                    VALUES (@Nome, @CPF, @RG, @DataNascimento, @Telefone, @Email, @Sexo, @EstadoCivil, @CEP, @Logradouro, @Numero, @Complemento, @CidadeId)";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nome", cliente.Nome);
                command.Parameters.AddWithValue("@CPF", cliente.CPF ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@RG", cliente.RG ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@DataNascimento", cliente.DataNascimento ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Telefone", cliente.Telefone ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Email", cliente.Email ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Sexo", cliente.Sexo ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@EstadoCivil", cliente.EstadoCivil ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CEP", cliente.CEP ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Logradouro", cliente.Logradouro ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Numero", cliente.Numero ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Complemento", cliente.Complemento ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CidadeId", cliente.CidadeId != 0 ? cliente.CidadeId : (object)DBNull.Value);

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar cliente: {ex.Message}");
                throw;
            }
        }
        public async Task Update(Cliente cliente)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            var query = "UPDATE clientes SET Nome=@Nome, CPF=@CPF, RG=@RG, DataNascimento=@DataNascimento, Telefone=@Telefone, Email=@Email, Sexo=@Sexo, EstadoCivil=@EstadoCivil, CEP=@CEP, Logradouro=@Logradouro, Numero=@Numero, Complemento=@Complemento, CidadeId=@CidadeId WHERE ClienteId=@ClienteId";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ClienteId", cliente.ClienteId);
            command.Parameters.AddWithValue("@Nome", cliente.Nome);
            command.Parameters.AddWithValue("@CPF", cliente.CPF);
            command.Parameters.AddWithValue("@RG", cliente.RG);
            command.Parameters.AddWithValue("@DataNascimento", (object)cliente.DataNascimento ?? DBNull.Value);
            command.Parameters.AddWithValue("@Telefone", cliente.Telefone);
            command.Parameters.AddWithValue("@Email", cliente.Telefone);
            command.Parameters.AddWithValue("@Sexo", cliente.Sexo);
            command.Parameters.AddWithValue("@EstadoCivil", cliente.EstadoCivil);
            command.Parameters.AddWithValue("@CEP", cliente.CEP);
            command.Parameters.AddWithValue("@Logradouro", cliente.Logradouro);
            command.Parameters.AddWithValue("@Numero", cliente.Numero);
            command.Parameters.AddWithValue("@Complemento", cliente.Complemento);
            command.Parameters.AddWithValue("@CidadeId", cliente.CidadeId);

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
            var clientes = new List<Cliente>();
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            var query = "SELECT * FROM clientes";

            using var command = new MySqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                clientes.Add(new Cliente
                {
                    ClienteId = reader.GetInt32("ClienteId"),
                    Nome = reader.GetString("Nome"),
                    CPF = reader.GetString("CPF"),
                    RG = reader.GetString("RG"),
                    DataNascimento = reader.IsDBNull(reader.GetOrdinal("DataNascimento")) ? (DateTime?)null : reader.GetDateTime("DataNascimento"),
                    Telefone = reader.GetString("Telefone"),
                    Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email"),
                    Sexo = reader.IsDBNull(reader.GetOrdinal("Sexo")) ? (char?)null : reader.GetChar("Sexo"),
                    EstadoCivil = reader.GetString("EstadoCivil"),
                    CEP = reader.GetString("CEP"),
                    Logradouro = reader.GetString("Logradouro"),
                    Numero = reader.GetString("Numero"),
                    Complemento = reader.GetString("Complemento"),
                    CidadeId = reader.IsDBNull(reader.GetOrdinal("CidadeId")) ? 0 : reader.GetInt32("CidadeId") // Verifique se CidadeId é nulo
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
                var cliente = new Cliente
                {
                    ClienteId = reader.GetInt32("ClienteId"),
                    Nome = reader.GetString("Nome"),
                    CPF = reader.GetString("CPF"),
                    RG = reader.GetString("RG"),
                    DataNascimento = reader.IsDBNull(reader.GetOrdinal("DataNascimento")) ? (DateTime?)null : reader.GetDateTime("DataNascimento"),
                    Telefone = reader.GetString("Telefone"),
                    Email = reader.GetString("Email"),
                    Sexo = reader.IsDBNull(reader.GetOrdinal("Sexo")) ? (char?)null : reader.GetChar("Sexo"),
                    EstadoCivil = reader.GetString("EstadoCivil"),
                    CEP = reader.GetString("CEP"),
                    Logradouro = reader.GetString("Logradouro"),
                    Numero = reader.GetString("Numero"),
                    Complemento = reader.GetString("Complemento"),
                    CidadeId = reader.GetInt32("CidadeId")
                };

                // Carregar o objeto OCidade usando o repositório de cidades
                if (cliente.CidadeId != 0)
                {
                    cliente.OCidade = await _cidadeRepository.GetById(cliente.CidadeId);
                }

                return cliente;
            }
            return null;
        }


        public async Task<List<Cliente>> GetByNome(string nome)
        {
            var clientes = new List<Cliente>();
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
                    DataNascimento = reader.IsDBNull(reader.GetOrdinal("DataNascimento")) ? (DateTime?)null : reader.GetDateTime("DataNascimento"),
                    Telefone = reader.GetString("Telefone"),
                    Email = reader.GetString("Email"),
                    Sexo = reader.IsDBNull(reader.GetOrdinal("Sexo")) ? (char?)null : reader.GetChar("Sexo"),
                    EstadoCivil = reader.GetString("EstadoCivil"),
                    CEP = reader.GetString("CEP"),
                    Logradouro = reader.GetString("Logradouro"),
                    Numero = reader.GetString("Numero"),
                    Complemento = reader.GetString("Complemento"),
                    CidadeId = reader.GetInt32("CidadeId")
                });
            }
            return clientes;
        }
    }
}
