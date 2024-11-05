using Modela.Data;
using Modela.Data.IBGERepositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

IPaisRepository paisRepository = new PaisIBGERepository();
IEstadoRepository estadoRepository = new EstadoIBGERepository(paisRepository);
ICidadeRepository cidadeRepository = new CidadeIBERepository(paisRepository, estadoRepository);

builder.Services.AddScoped<IPaisRepository>(provider => paisRepository);
builder.Services.AddScoped<IEstadoRepository>(provider => estadoRepository);
builder.Services.AddScoped<ICidadeRepository>(provider => cidadeRepository);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
