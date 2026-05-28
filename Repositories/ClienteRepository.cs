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
                
                // SQL optimizado para tus nuevos campos
                string query = "INSERT INTO Clientes (Nombre, Apellido, Telefono, Email, IdRol) VALUES (@nombre, @apellido, @telefono, @email, @idRol)";
                
                using (var comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", cliente.Nombre);
                    comando.Parameters.AddWithValue("@apellido", cliente.Apellido);
                    comando.Parameters.AddWithValue("@telefono", cliente.Telefono);
                    comando.Parameters.AddWithValue("@email", cliente.Email);
                    comando.Parameters.AddWithValue("@idRol", cliente.IdRol);

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
                string query = "SELECT IdCliente, Nombre, Apellido, Telefono, Email, IdRol, FechaCreacion FROM Clientes";
                
                using (var comando = new MySqlCommand(query, conexion))
                using (var lector = await comando.ExecuteReaderAsync())
                {
                    while (await lector.ReadAsync())
                    {
                        clientes.Add(new Cliente
                        {
                            IdCliente = Convert.ToInt32(lector["IdCliente"]),
                            Nombre = lector["Nombre"].ToString(),
                            Apellido = lector["Apellido"].ToString(),
                            Telefono = lector["Telefono"].ToString(),
                            Email = lector["Email"].ToString(),
                            IdRol = Convert.ToInt32(lector["IdRol"]),
                            FechaCreacion = Convert.ToDateTime(lector["FechaCreacion"])
                        });
                    }
                }
            }
            return clientes;
        }
    }
}