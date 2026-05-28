using Legendary_Motorsport_Backend.Repositories;
using Legendary_Motorsport_Backend.Models;

namespace Legendary_Motorsport_Backend.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly string _jwtKey;
        private readonly int _jwtExpirationMinutes;

        // DIP: Pedimos la interfaz del repositorio, no la clase concreta
        public ClienteService(IClienteRepository clienteRepository, IConfiguration configuration)
        {
            _clienteRepository = clienteRepository;
            _jwtIssuer = configuration["JwtSettings:Issuer"] ?? string.Empty;
            _jwtAudience = configuration["JwtSettings:Audience"] ?? string.Empty;
            _jwtKey = configuration["JwtSettings:Key"] ?? string.Empty;
            _jwtExpirationMinutes = int.TryParse(configuration["JwtSettings:ExpirationMinutes"], out var minutes)
                ? minutes
                : 120;
        }

        public async Task<bool> RegistrarClienteAsync(ClienteCreateRequest cliente)
        {
            // SRP: Aquí aplicamos las reglas de negocio antes de tocar la base de datos
            if (string.IsNullOrWhiteSpace(cliente.Email))
            {
                throw new ArgumentException("El email es obligatorio para registrar un cliente.");
            }

            if (string.IsNullOrWhiteSpace(cliente.Contrasena))
            {
                throw new ArgumentException("La contraseña es obligatoria para registrar un cliente.");
            }

            var hash = BCrypt.Net.BCrypt.HashPassword(cliente.Contrasena);
            var entidad = new Cliente
            {
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Telefono = cliente.Telefono,
                Email = cliente.Email,
                IdRol = cliente.IdRol,
                ContrasenaHash = hash
            };

            return await _clienteRepository.CrearClienteAsync(entidad);
        }

        public async Task<IEnumerable<Cliente>> ObtenerTodosAsync()
        {
            return await _clienteRepository.ObtenerTodosAsync();
        }

        public async Task<Cliente?> ObtenerPorIdAsync(int idCliente)
        {
            return await _clienteRepository.ObtenerPorIdAsync(idCliente);
        }

        public async Task<bool> ActualizarClienteAsync(int idCliente, ClienteUpdateRequest cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.Email))
            {
                throw new ArgumentException("El email es obligatorio para actualizar un cliente.");
            }

            var actualizarContrasena = !string.IsNullOrWhiteSpace(cliente.Contrasena);
            var entidad = new Cliente
            {
                IdCliente = idCliente,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Telefono = cliente.Telefono,
                Email = cliente.Email,
                IdRol = cliente.IdRol,
                ContrasenaHash = actualizarContrasena
                    ? BCrypt.Net.BCrypt.HashPassword(cliente.Contrasena)
                    : string.Empty
            };

            return await _clienteRepository.ActualizarClienteAsync(entidad, actualizarContrasena);
        }

        public async Task<bool> EliminarClienteAsync(int idCliente)
        {
            return await _clienteRepository.EliminarClienteAsync(idCliente);
        }

        public async Task<LoginResponse?> LoginAsync(ClienteLoginRequest login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Contrasena))
            {
                throw new ArgumentException("Email y contraseña son obligatorios.");
            }

            var cliente = await _clienteRepository.ObtenerPorEmailAsync(login.Email);
            if (cliente == null)
            {
                return null;
            }

            var valido = BCrypt.Net.BCrypt.Verify(login.Contrasena, cliente.ContrasenaHash);
            if (!valido)
            {
                return null;
            }

            var token = GenerarToken(cliente);
            return new LoginResponse
            {
                Token = token,
                IdCliente = cliente.IdCliente,
                Email = cliente.Email,
                IdRol = cliente.IdRol
            };
        }

        private string GenerarToken(Cliente cliente)
        {
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, cliente.IdCliente.ToString()),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, cliente.Email),
                new System.Security.Claims.Claim("role_id", cliente.IdRol.ToString()),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, cliente.IdRol.ToString())
            };

            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes),
                signingCredentials: creds);

            return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}