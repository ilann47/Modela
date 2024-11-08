using Modela.Data.IBGERepositories;
using Modela.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Modela.Data.MySQLRepositories {
    public class ClienteMySQLRepository : IClienteRepository {
        private readonly string _connectionString;
        private readonly ICidadeRepository _cidadeRepository;

        public ClienteMySQLRepository(string connectionString, ICidadeRepository cidadeRepository) {
            _connectionString = connectionString;
            _cidadeRepository = cidadeRepository;
        }

        private object DBNullIfEmpty(string? value) => string.IsNullOrWhiteSpace(value) ? DBNull.Value : value;

        public async Task Add(Cliente cliente) {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = @"
                INSERT INTO clientes (Nome, CPF, RG, DataNascimento, Telefone, Email, Sexo, EstadoCivil, CEP, Logradouro, Numero, Complemento, Cidade)
                VALUES (@Nome, @CPF, @RG, @DataNascimento, @Telefone, @Email, @Sexo, @EstadoCivil, @CEP, @Logradouro, @Numero, @Complemento, @Cidade)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Nome", DBNullIfEmpty(cliente.Nome));
            command.Parameters.AddWithValue("@CPF", DBNullIfEmpty(cliente.CPF));
            command.Parameters.AddWithValue("@RG", DBNullIfEmpty(cliente.RG));
            command.Parameters.AddWithValue("@DataNascimento", cliente.DataNascimento == DateTime.MinValue ? (object) DBNull.Value : cliente.DataNascimento);
            command.Parameters.AddWithValue("@Telefone", DBNullIfEmpty(cliente.Telefone));
            command.Parameters.AddWithValue("@Email", DBNullIfEmpty(cliente.Email));
            command.Parameters.AddWithValue("@Sexo", cliente.Sexo);
            command.Parameters.AddWithValue("@EstadoCivil", DBNullIfEmpty(cliente.EstadoCivil));
            command.Parameters.AddWithValue("@CEP", DBNullIfEmpty(cliente.CEP));
            command.Parameters.AddWithValue("@Logradouro", DBNullIfEmpty(cliente.Logradouro));
            command.Parameters.AddWithValue("@Numero", DBNullIfEmpty(cliente.Numero));
            command.Parameters.AddWithValue("@Complemento", DBNullIfEmpty(cliente.Complemento));
            command.Parameters.AddWithValue("@Cidade", DBNullIfEmpty(cliente.Cidade));

            await command.ExecuteNonQueryAsync();
        }

        public async Task Update(Cliente cliente) {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = @"
                UPDATE clientes SET Nome=@Nome, CPF=@CPF, RG=@RG, DataNascimento=@DataNascimento, Telefone=@Telefone, Email=@Email, 
                Sexo=@Sexo, EstadoCivil=@EstadoCivil, CEP=@CEP, Logradouro=@Logradouro, Numero=@Numero, Complemento=@Complemento, 
                Cidade=@Cidade WHERE ClienteId=@ClienteId";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ClienteId", cliente.ClienteId);
            command.Parameters.AddWithValue("@Nome", DBNullIfEmpty(cliente.Nome));
            command.Parameters.AddWithValue("@CPF", DBNullIfEmpty(cliente.CPF));
            command.Parameters.AddWithValue("@RG", DBNullIfEmpty(cliente.RG));
            command.Parameters.AddWithValue("@DataNascimento", cliente.DataNascimento == DateTime.MinValue ? (object) DBNull.Value : cliente.DataNascimento);
            command.Parameters.AddWithValue("@Telefone", DBNullIfEmpty(cliente.Telefone));
            command.Parameters.AddWithValue("@Email", DBNullIfEmpty(cliente.Email));
            command.Parameters.AddWithValue("@Sexo", cliente.Sexo);
            command.Parameters.AddWithValue("@EstadoCivil", DBNullIfEmpty(cliente.EstadoCivil));
            command.Parameters.AddWithValue("@CEP", DBNullIfEmpty(cliente.CEP));
            command.Parameters.AddWithValue("@Logradouro", DBNullIfEmpty(cliente.Logradouro));
            command.Parameters.AddWithValue("@Numero", DBNullIfEmpty(cliente.Numero));
            command.Parameters.AddWithValue("@Complemento", DBNullIfEmpty(cliente.Complemento));
            command.Parameters.AddWithValue("@Cidade", DBNullIfEmpty(cliente.Cidade));

            await command.ExecuteNonQueryAsync();
        }

        private string EmptyIfDBNull(object? value) => value == DBNull.Value ? string.Empty : value?.ToString() ?? string.Empty;

        public async Task<List<Cliente>> GetTodos() {
            var clientes = new List<Cliente>();
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = "SELECT * FROM clientes";
            using var command = new MySqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync()) {
                var cliente = new Cliente {
                    ClienteId = reader.GetInt32("ClienteId"),
                    Nome = EmptyIfDBNull(reader["Nome"]),
                    CPF = EmptyIfDBNull(reader["CPF"]),
                    RG = EmptyIfDBNull(reader["RG"]),
                    DataNascimento = reader["DataNascimento"] == DBNull.Value ? DateTime.MinValue : (DateTime) reader["DataNascimento"],
                    Telefone = EmptyIfDBNull(reader["Telefone"]),
                    Email = EmptyIfDBNull(reader["Email"]),
                    Sexo = reader.IsDBNull(reader.GetOrdinal("Sexo")) ? ' ' : reader.GetChar("Sexo"),
                    EstadoCivil = EmptyIfDBNull(reader["EstadoCivil"]),
                    CEP = EmptyIfDBNull(reader["CEP"]),
                    Logradouro = EmptyIfDBNull(reader["Logradouro"]),
                    Numero = EmptyIfDBNull(reader["Numero"]),
                    Complemento = EmptyIfDBNull(reader["Complemento"]),
                    Cidade = EmptyIfDBNull(reader["Cidade"])
                };

                clientes.Add(cliente);
            }
            return clientes;
        }

        public async Task<Cliente?> GetById(int id) {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = "SELECT * FROM clientes WHERE ClienteId = @id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync()) {
                return new Cliente {
                    ClienteId = reader.GetInt32("ClienteId"),
                    Nome = EmptyIfDBNull(reader["Nome"]),
                    CPF = EmptyIfDBNull(reader["CPF"]),
                    RG = EmptyIfDBNull(reader["RG"]),
                    DataNascimento = reader["DataNascimento"] == DBNull.Value ? DateTime.MinValue : (DateTime) reader["DataNascimento"],
                    Telefone = EmptyIfDBNull(reader["Telefone"]),
                    Email = EmptyIfDBNull(reader["Email"]),
                    Sexo = reader.IsDBNull(reader.GetOrdinal("Sexo")) ? ' ' : reader.GetChar("Sexo"),
                    EstadoCivil = EmptyIfDBNull(reader["EstadoCivil"]),
                    CEP = EmptyIfDBNull(reader["CEP"]),
                    Logradouro = EmptyIfDBNull(reader["Logradouro"]),
                    Numero = EmptyIfDBNull(reader["Numero"]),
                    Complemento = EmptyIfDBNull(reader["Complemento"]),
                    Cidade = EmptyIfDBNull(reader["Cidade"])
                };
            }
            return null;
        }

        public async Task<List<Cliente>> GetByNome(string nome) {
            var clientes = new List<Cliente>();
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = "SELECT * FROM clientes WHERE LOWER(Nome) LIKE LOWER(@nome)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@nome", $"%{nome}%");

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync()) {
                var cliente = new Cliente {
                    ClienteId = reader.GetInt32("ClienteId"),
                    Nome = EmptyIfDBNull(reader["Nome"]),
                    CPF = EmptyIfDBNull(reader["CPF"]),
                    RG = EmptyIfDBNull(reader["RG"]),
                    DataNascimento = reader["DataNascimento"] == DBNull.Value ? DateTime.MinValue : (DateTime) reader["DataNascimento"],
                    Telefone = EmptyIfDBNull(reader["Telefone"]),
                    Email = EmptyIfDBNull(reader["Email"]),
                    Sexo = reader.IsDBNull(reader.GetOrdinal("Sexo")) ? ' ' : reader.GetChar("Sexo"),
                    EstadoCivil = EmptyIfDBNull(reader["EstadoCivil"]),
                    CEP = EmptyIfDBNull(reader["CEP"]),
                    Logradouro = EmptyIfDBNull(reader["Logradouro"]),
                    Numero = EmptyIfDBNull(reader["Numero"]),
                    Complemento = EmptyIfDBNull(reader["Complemento"]),
                    Cidade = EmptyIfDBNull(reader["Cidade"])
                };

                clientes.Add(cliente);
            }
            return clientes;
        }

        public async Task Delete(int id) {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = "DELETE FROM clientes WHERE ClienteId = @ClienteId";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ClienteId", id);

            int rowsAffected = await command.ExecuteNonQueryAsync();
        }
    }
}
