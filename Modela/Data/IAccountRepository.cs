using System.Threading.Tasks;
using Modela.Models;

namespace Modela.Data
{
    public interface IAccountRepository
    {
        Task<Account> GetAccount(string email);
        Task<Account> ValidateLoginAsync(string usernameOrEmail, string password);
    }
}
