using Legendary_Motorsport_Backend.Data;
using Legendary_Motorsport_Backend.Repositories;
using Legendary_Motorsport_Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar soporte para Controladores
builder.Services.AddControllers();

// 2. Inyección de Dependencias (Aplicando DIP)
// Conexión a la Base de Datos
builder.Services.AddSingleton<IConexionDb, ConexionDb>();

// Repositorios (Capa de Datos)
builder.Services.AddScoped<IVehiculoRepository, VehiculoRepository>(); 
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

// Servicios (Capa de Lógica de Negocio)
builder.Services.AddScoped<IClienteService, ClienteService>();

// 3. Configuración de Swagger (Para probar la API visualmente)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Configuración del Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// 5. Activar las rutas de los controladores
app.MapControllers();

app.Run();