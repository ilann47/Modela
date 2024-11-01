using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços ao contêiner.
builder.Services.AddControllersWithViews();

// Configura a conexão com MySQL como um serviço
builder.Services.AddScoped<MySqlConnection>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
    return new MySqlConnection(connectionString);
});

var app = builder.Build(); // Essa linha agora vem depois da configuração de serviços.

// Configura o pipeline de requisições HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
