using Modela.Models;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Modela.Data.MySQLRepositories
{
    public class AccountMYSQLRepository : IAccountRepository
    {
        private readonly string _connectionString;

        public AccountMYSQLRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Account> GetAccount(string email)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                string query = "SELECT * FROM usuarios WHERE email = @Email";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);

                using var reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                {
                    return new Account
                    {
                        Id = reader.GetInt32("id"),
                        Email = reader.GetString("email"),
                        Name = reader.GetString("nome"),
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter a conta: {ex.Message}");
            }

            return null;
        }

        public async Task<Account> ValidateLoginAsync(string usernameOrEmail, string password)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                string query = "SELECT * FROM usuarios WHERE (email = @Email OR nome = @Nome) AND senha = @Senha";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", usernameOrEmail);
                command.Parameters.AddWithValue("@Nome", usernameOrEmail);
                command.Parameters.AddWithValue("@Senha", password);

                using var reader = await command.ExecuteReaderAsync();
                if (reader.Read())
                {
                    return new Account
                    {
                        Id = reader.GetInt32("id"),
                        Email = reader.GetString("email"),
                        Name = reader.GetString("nome"),
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao validar login: {ex.Message}");
            }

            return null;
        }
    }
}
