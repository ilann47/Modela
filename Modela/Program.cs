using Modela.Data;
using Modela.Data.IBGERepositories;
using Modela.Data.MySQLRepositories;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IPaisRepository, PaisIBGERepository>();
builder.Services.AddScoped<IEstadoRepository, EstadoIBGERepository>();
builder.Services.AddScoped<ICidadeRepository, CidadeIBERepository>();

builder.Services.AddScoped<IAccountRepository>(provider =>
    new AccountMYSQLRepository(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login"; // Rota de login
        options.LogoutPath = "/Account/Logout"; // Rota de logout
        options.AccessDeniedPath = "/Account/AccessDenied"; // Rota para acesso negado
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // Middleware de autenticação
app.UseAuthorization();  // Middleware de autorização

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); 

app.Run();
