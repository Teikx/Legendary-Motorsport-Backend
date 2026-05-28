using Legendary_Motorsport_Backend.Data;
using Legendary_Motorsport_Backend.Repositories;
using Legendary_Motorsport_Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar soporte para Controladores
builder.Services.AddControllers();

// CORS para permitir llamadas desde el front (incluye preflight OPTIONS)
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendDev", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// 2. Inyección de Dependencias (Aplicando DIP)
// Conexión a la Base de Datos
builder.Services.AddSingleton<IConexionDb, ConexionDb>();

// Repositorios (Capa de Datos)
//builder.Services.AddScoped<IVehiculoRepository, VehiculoRepository>(); 
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

// Servicios (Capa de Lógica de Negocio)
builder.Services.AddScoped<IClienteService, ClienteService>();

// Autenticacion y autorizacion con JWT
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
var jwtAudience = builder.Configuration["JwtSettings:Audience"];
var jwtKey = builder.Configuration["JwtSettings:Key"];
var adminRoleId = builder.Configuration["JwtSettings:AdminRoleId"] ?? "1";

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey ?? string.Empty))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy =>
        policy.RequireClaim("role_id", adminRoleId));
});

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

app.UseCors("FrontendDev");

app.UseAuthentication();
app.UseAuthorization();

// 5. Activar las rutas de los controladores
app.MapControllers();

app.Run();