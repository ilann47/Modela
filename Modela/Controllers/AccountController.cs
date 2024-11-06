using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Modela.Data;
using Modela.Models;
using System.Security.Claims;
using System.Threading.Tasks;

[Route("Account")]
public class AccountController : Controller
{
    private readonly IAccountRepository _accountRepository;

    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpGet("Login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(string usernameOrEmail, string password)
    {
        var account = await _accountRepository.ValidateLoginAsync(usernameOrEmail, password);
        if (account != null)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, account.Name),
            new Claim(ClaimTypes.Email, account.Email)
        };

            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");

            // Define IsPersistent como false para criar um cookie de sessão
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false // Isso garante que o cookie expire ao fechar o navegador
            };

            await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

            TempData["SuccessMessage"] = "Login bem-sucedido!";
            return RedirectToAction("Index", "Home");
        }

        TempData["ErrorMessage"] = "Usuário ou senha incorretos. Tente novamente.";
        return RedirectToAction("Login");
    }


    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        // Realiza o logout do usuário, removendo o cookie de autenticação
        await HttpContext.SignOutAsync("CookieAuth");


        // Redireciona para a página de login
        return RedirectToAction("Login", "Account");
    }

}
