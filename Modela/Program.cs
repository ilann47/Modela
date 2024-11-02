using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Modela.Services;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);

// Configuração de serviços
builder.Services.AddControllersWithViews();

// Registra a implementação do serviço IClienteService
builder.Services.AddScoped<IClienteService, ClienteService>();

// Configura a conexão com MySQL como um serviço
builder.Services.AddScoped<MySqlConnection>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
    return new MySqlConnection(connectionString);
});

var app = builder.Build();

// Configura o pipeline de requisições HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

// Configuração da rota padrão, permitindo o acesso ao ClienteController
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
