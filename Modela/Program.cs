using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Modela.Data;
using Modela.Data.IBGERepositories;
using Modela.Data.MySQLRepositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configuração de serviços
builder.Services.AddControllersWithViews();

// Repositórios IBGE
builder.Services.AddScoped<IPaisRepository, PaisIBGERepository>();
builder.Services.AddScoped<IEstadoRepository, EstadoIBGERepository>();
builder.Services.AddScoped<ICidadeRepository, CidadeIBERepository>();

// Repositórios MySQL
builder.Services.AddScoped<IAccountRepository>(provider =>
    new AccountMYSQLRepository(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IClienteRepository>(provider =>
    new ClienteMySQLRepository(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Configuração de autenticação e autorização
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login"; // Rota de login
        options.LogoutPath = "/Account/Logout"; // Rota de logout
        options.AccessDeniedPath = "/Account/AccessDenied"; // Rota para acesso negado
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configuração de segurança e middleware
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Middleware de autenticação e autorização
app.UseAuthentication(); // Middleware de autenticação deve vir primeiro
app.UseAuthorization();  // Middleware de autorização

// Configuração das rotas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
