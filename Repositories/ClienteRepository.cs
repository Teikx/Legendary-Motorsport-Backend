using MySqlConnector;
using Legendary_Motorsport_Backend.Data;
using Legendary_Motorsport_Backend.Models;

namespace Legendary_Motorsport_Backend.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly IConexionDb _conexionDb;

        // Inyectamos la conexión mediante su interfaz (DIP)
        public ClienteRepository(IConexionDb conexionDb)
        {
            _conexionDb = conexionDb;
        }

        public async Task<bool> CrearClienteAsync(Cliente cliente)
        {
            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();
                
                string query = "INSERT INTO Cliente (nombre, apellido, telefono, email, IdRol, contrasena) VALUES (@nombre, @apellido, @telefono, @email, @idRol, @contrasena)";
                
                using (var comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", cliente.Nombre);
                    comando.Parameters.AddWithValue("@apellido", cliente.Apellido);
                    comando.Parameters.AddWithValue("@telefono", cliente.Telefono);
                    comando.Parameters.AddWithValue("@email", cliente.Email);
                    comando.Parameters.AddWithValue("@idRol", cliente.IdRol);
                    comando.Parameters.AddWithValue("@contrasena", cliente.ContrasenaHash);

                    int filasAfectadas = await comando.ExecuteNonQueryAsync();
                    return filasAfectadas > 0;
                }
            }
        }

        public async Task<IEnumerable<Cliente>> ObtenerTodosAsync()
        {
            var clientes = new List<Cliente>();

            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();
                string query = "SELECT IdCliente, nombre, apellido, telefono, email, IdRol, fechaCreacion FROM Cliente";
                
                using (var comando = new MySqlCommand(query, conexion))
                using (var lector = await comando.ExecuteReaderAsync())
                {
                    while (await lector.ReadAsync())
                    {
                        clientes.Add(new Cliente
                        {
                            IdCliente = Convert.ToInt32(lector["IdCliente"]),
                            Nombre = lector["nombre"].ToString(),
                            Apellido = lector["apellido"].ToString(),
                            Telefono = lector["telefono"].ToString(),
                            Email = lector["email"].ToString(),
                            IdRol = Convert.ToInt32(lector["IdRol"]),
                            FechaCreacion = Convert.ToDateTime(lector["fechaCreacion"])
                        });
                    }
                }
            }
            return clientes;
        }

        public async Task<Cliente?> ObtenerPorIdAsync(int idCliente)
        {
            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();
                string query = "SELECT IdCliente, nombre, apellido, telefono, email, IdRol, fechaCreacion FROM Cliente WHERE IdCliente = @idCliente";

                using (var comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idCliente", idCliente);
                    using (var lector = await comando.ExecuteReaderAsync())
                    {
                        if (await lector.ReadAsync())
                        {
                            return new Cliente
                            {
                                IdCliente = Convert.ToInt32(lector["IdCliente"]),
                                Nombre = lector["nombre"].ToString(),
                                Apellido = lector["apellido"].ToString(),
                                Telefono = lector["telefono"].ToString(),
                                Email = lector["email"].ToString(),
                                IdRol = Convert.ToInt32(lector["IdRol"]),
                                FechaCreacion = Convert.ToDateTime(lector["fechaCreacion"])
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<Cliente?> ObtenerPorEmailAsync(string email)
        {
            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();
                string query = "SELECT IdCliente, nombre, apellido, telefono, email, IdRol, fechaCreacion, contrasena FROM Cliente WHERE email = @email";

                using (var comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@email", email);
                    using (var lector = await comando.ExecuteReaderAsync())
                    {
                        if (await lector.ReadAsync())
                        {
                            return new Cliente
                            {
                                IdCliente = Convert.ToInt32(lector["IdCliente"]),
                                Nombre = lector["nombre"].ToString(),
                                Apellido = lector["apellido"].ToString(),
                                Telefono = lector["telefono"].ToString(),
                                Email = lector["email"].ToString(),
                                IdRol = Convert.ToInt32(lector["IdRol"]),
                                FechaCreacion = Convert.ToDateTime(lector["fechaCreacion"]),
                                ContrasenaHash = lector["contrasena"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<bool> ActualizarClienteAsync(Cliente cliente, bool actualizarContrasena)
        {
            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();
                string query = actualizarContrasena
                    ? "UPDATE Cliente SET nombre = @nombre, apellido = @apellido, telefono = @telefono, email = @email, IdRol = @idRol, contrasena = @contrasena WHERE IdCliente = @idCliente"
                    : "UPDATE Cliente SET nombre = @nombre, apellido = @apellido, telefono = @telefono, email = @email, IdRol = @idRol WHERE IdCliente = @idCliente";

                using (var comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", cliente.Nombre);
                    comando.Parameters.AddWithValue("@apellido", cliente.Apellido);
                    comando.Parameters.AddWithValue("@telefono", cliente.Telefono);
                    comando.Parameters.AddWithValue("@email", cliente.Email);
                    comando.Parameters.AddWithValue("@idRol", cliente.IdRol);
                    comando.Parameters.AddWithValue("@idCliente", cliente.IdCliente);

                    if (actualizarContrasena)
                    {
                        comando.Parameters.AddWithValue("@contrasena", cliente.ContrasenaHash);
                    }

                    int filasAfectadas = await comando.ExecuteNonQueryAsync();
                    return filasAfectadas > 0;
                }
            }
        }

        public async Task<bool> EliminarClienteAsync(int idCliente)
        {
            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();
                string query = "DELETE FROM Cliente WHERE IdCliente = @idCliente";

                using (var comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idCliente", idCliente);
                    int filasAfectadas = await comando.ExecuteNonQueryAsync();
                    return filasAfectadas > 0;
                }
            }
        }
    }
}