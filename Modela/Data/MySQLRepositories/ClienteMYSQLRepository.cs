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
            command.Parameters.AddWithValue("@Nome", DBNullIfEmpty(cliente.Nome.ToUpper()));
            command.Parameters.AddWithValue("@CPF", DBNullIfEmpty(cliente.CPF.ToUpper()));
            command.Parameters.AddWithValue("@RG", DBNullIfEmpty(cliente.RG.ToUpper()));
            command.Parameters.AddWithValue("@DataNascimento", cliente.DataNascimento == DateTime.MinValue ? (object) DBNull.Value : cliente.DataNascimento);
            command.Parameters.AddWithValue("@Telefone", DBNullIfEmpty(cliente.Telefone.ToUpper()));
            command.Parameters.AddWithValue("@Email", DBNullIfEmpty(cliente.Email.ToUpper()));
            command.Parameters.AddWithValue("@Sexo", char.ToUpper(cliente.Sexo));
            command.Parameters.AddWithValue("@EstadoCivil", DBNullIfEmpty(cliente.EstadoCivil.ToUpper()));
            command.Parameters.AddWithValue("@CEP", DBNullIfEmpty(cliente.CEP.ToUpper()));
            command.Parameters.AddWithValue("@Logradouro", DBNullIfEmpty(cliente.Logradouro.ToUpper()));
            command.Parameters.AddWithValue("@Numero", DBNullIfEmpty(cliente.Numero.ToUpper()));
            command.Parameters.AddWithValue("@Complemento", DBNullIfEmpty(cliente.Complemento.ToUpper()));
            command.Parameters.AddWithValue("@Cidade", DBNullIfEmpty(cliente.Cidade.ToUpper()));

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
            command.Parameters.AddWithValue("@Nome", DBNullIfEmpty(cliente.Nome.ToUpper()));
            command.Parameters.AddWithValue("@CPF", DBNullIfEmpty(cliente.CPF.ToUpper()));
            command.Parameters.AddWithValue("@RG", DBNullIfEmpty(cliente.RG.ToUpper()));
            command.Parameters.AddWithValue("@DataNascimento", cliente.DataNascimento == DateTime.MinValue ? (object) DBNull.Value : cliente.DataNascimento);
            command.Parameters.AddWithValue("@Telefone", DBNullIfEmpty(cliente.Telefone.ToUpper()));
            command.Parameters.AddWithValue("@Email", DBNullIfEmpty(cliente.Email.ToUpper()));
            command.Parameters.AddWithValue("@Sexo", char.ToUpper(cliente.Sexo));
            command.Parameters.AddWithValue("@EstadoCivil", DBNullIfEmpty(cliente.EstadoCivil.ToUpper()));
            command.Parameters.AddWithValue("@CEP", DBNullIfEmpty(cliente.CEP.ToUpper()));
            command.Parameters.AddWithValue("@Logradouro", DBNullIfEmpty(cliente.Logradouro.ToUpper()));
            command.Parameters.AddWithValue("@Numero", DBNullIfEmpty(cliente.Numero.ToUpper()));
            command.Parameters.AddWithValue("@Complemento", DBNullIfEmpty(cliente.Complemento.ToUpper()));
            command.Parameters.AddWithValue("@Cidade", DBNullIfEmpty(cliente.Cidade.ToUpper()));

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
                    Nome = EmptyIfDBNull(reader["Nome"]).ToUpper(),
                    CPF = EmptyIfDBNull(reader["CPF"]).ToUpper(),
                    RG = EmptyIfDBNull(reader["RG"]).ToUpper(),
                    DataNascimento = reader["DataNascimento"] == DBNull.Value ? DateTime.MinValue : (DateTime) reader["DataNascimento"],
                    Telefone = EmptyIfDBNull(reader["Telefone"]).ToUpper(),
                    Email = EmptyIfDBNull(reader["Email"]).ToUpper(),
                    Sexo = char.ToUpper(reader.IsDBNull(reader.GetOrdinal("Sexo")) ? ' ' : reader.GetChar("Sexo")),
                    EstadoCivil = EmptyIfDBNull(reader["EstadoCivil"]).ToUpper(),
                    CEP = EmptyIfDBNull(reader["CEP"]).ToUpper(),
                    Logradouro = EmptyIfDBNull(reader["Logradouro"]).ToUpper(),
                    Numero = EmptyIfDBNull(reader["Numero"]).ToUpper(),
                    Complemento = EmptyIfDBNull(reader["Complemento"]).ToUpper(),
                    Cidade = EmptyIfDBNull(reader["Cidade"]).ToUpper()
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
                    Nome = EmptyIfDBNull(reader["Nome"]).ToUpper(),
                    CPF = EmptyIfDBNull(reader["CPF"]).ToUpper(),
                    RG = EmptyIfDBNull(reader["RG"]).ToUpper(),
                    DataNascimento = reader["DataNascimento"] == DBNull.Value ? DateTime.MinValue : (DateTime) reader["DataNascimento"],
                    Telefone = EmptyIfDBNull(reader["Telefone"]).ToUpper(),
                    Email = EmptyIfDBNull(reader["Email"]).ToUpper(),
                    Sexo = char.ToUpper(reader.IsDBNull(reader.GetOrdinal("Sexo")) ? ' ' : reader.GetChar("Sexo")),
                    EstadoCivil = EmptyIfDBNull(reader["EstadoCivil"]).ToUpper(),
                    CEP = EmptyIfDBNull(reader["CEP"]).ToUpper(),
                    Logradouro = EmptyIfDBNull(reader["Logradouro"]).ToUpper(),
                    Numero = EmptyIfDBNull(reader["Numero"]).ToUpper(),
                    Complemento = EmptyIfDBNull(reader["Complemento"]).ToUpper(),
                    Cidade = EmptyIfDBNull(reader["Cidade"]).ToUpper()
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
                    Nome = EmptyIfDBNull(reader["Nome"]).ToUpper(),
                    CPF = EmptyIfDBNull(reader["CPF"]).ToUpper(),
                    RG = EmptyIfDBNull(reader["RG"]).ToUpper(),
                    DataNascimento = reader["DataNascimento"] == DBNull.Value ? DateTime.MinValue : (DateTime) reader["DataNascimento"],
                    Telefone = EmptyIfDBNull(reader["Telefone"]).ToUpper(),
                    Email = EmptyIfDBNull(reader["Email"]).ToUpper(),
                    Sexo = char.ToUpper(reader.IsDBNull(reader.GetOrdinal("Sexo")) ? ' ' : reader.GetChar("Sexo")),
                    EstadoCivil = EmptyIfDBNull(reader["EstadoCivil"]).ToUpper(),
                    CEP = EmptyIfDBNull(reader["CEP"]).ToUpper(),
                    Logradouro = EmptyIfDBNull(reader["Logradouro"]).ToUpper(),
                    Numero = EmptyIfDBNull(reader["Numero"]).ToUpper(),
                    Complemento = EmptyIfDBNull(reader["Complemento"]).ToUpper(),
                    Cidade = EmptyIfDBNull(reader["Cidade"]).ToUpper()
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
