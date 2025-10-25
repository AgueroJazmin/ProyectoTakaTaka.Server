using Microsoft.EntityFrameworkCore;
using ProyectoTakaTaka.BD.Datos;
using ProyectoTakaTaka.BD.Datos.Entity;
using ProyectoTakaTaka.Repositorio.Repositorios;
using ProyectoTakaTaka.Server.Client.Pages;
using ProyectoTakaTaka.Server.Components;
using ProyectoTakaTaka.Servicio.ServicioHttp;
using ProyectoTakaTaka.Shared.Configuraciones;

//Configura el constructor de la aplicacion
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri("https://localhost:7292") });
builder.Services.AddScoped<IHttpServicio, HttpServicio>();

// Agregar controladores (API)
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
});

builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("ConSQL")
    ?? throw new InvalidOperationException("El string de conexión no existe.");


builder.Services.AddDbContext<MiDbContext>(options =>
    options.UseSqlServer(connectionString));

// Registrar repositorios
builder.Services.AddScoped<IRepositorio<Evento>, Repositorio<Evento>>();
builder.Services.AddScoped<IRepositorioEvento, RepositorioEvento>();
builder.Services.AddScoped<IRepositorioCliente, RepositorioCliente>();
builder.Services.AddScoped<IRepositorioCombo, RepositorioCombo>();
builder.Services.AddScoped<IRepositorioOpcional, RepositorioOpcional>();
builder.Services.AddScoped<IRepositorioCumpleanero, RepositorioCumpleanero>();
builder.Services.AddScoped<IRepositorioHorario, RepositorioHorario>();
builder.Services.AddScoped<IRepositorioPago, RepositorioPago>();



// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddServerSideBlazor()
      .AddCircuitOptions(options => { options.DetailedErrors = true; });

//Construccion de la aplicacion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();

    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ProyectoTakaTaka.Server.Client._Imports).Assembly);

app.MapControllers();

app.Run();
