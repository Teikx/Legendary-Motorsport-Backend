using MySqlConnector;
using Legendary_Motorsport_Backend.Data;
using Legendary_Motorsport_Backend.Models;

namespace Legendary_Motorsport_Backend.Repositories
{
    public class DireccionRepository : IDireccionRepository
    {
        private readonly IConexionDb _conexionDb;

        public DireccionRepository(IConexionDb conexionDb)
        {
            _conexionDb = conexionDb;
        }

        public async Task<IEnumerable<Direccion>> ObtenerPorClienteIdAsync(int idCliente)
        {
            var direcciones = new List<Direccion>();
            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();
                // Pedimos explícitamente las columnas de tu BD
                string query = "SELECT IdDire, IdCliente, Nombre, Direccion, ciudad, region, codigo_postal, pais FROM Direcciones WHERE IdCliente = @idCliente";
                
                using (var comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idCliente", idCliente);
                    using (var lector = await comando.ExecuteReaderAsync())
                    {
                        while (await lector.ReadAsync())
                        {
                            direcciones.Add(new Direccion
                            {
                                IdDire = Convert.ToInt32(lector["IdDire"]),
                                IdCliente = Convert.ToInt32(lector["IdCliente"]),
                                Nombre = Convert.ToString(lector["Nombre"]),
                                DetalleDireccion = Convert.ToString(lector["Direccion"]), // Mapeo a tu propiedad
                                Ciudad = Convert.ToString(lector["ciudad"]),
                                Region = Convert.ToString(lector["region"]),
                                CodigoPostal = Convert.ToString(lector["codigo_postal"]),
                                Pais = Convert.ToString(lector["pais"])
                            });
                        }
                    }
                }
            }
            return direcciones;
        }

        public async Task<bool> CrearAsync(Direccion direccion)
        {
            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();
                string query = "INSERT INTO Direcciones (IdCliente, Nombre, Direccion, ciudad, region, codigo_postal, pais) VALUES (@idCliente, @nombre, @detalleDireccion, @ciudad, @region, @codigoPostal, @pais)";
                
                using (var comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idCliente", direccion.IdCliente);
                    comando.Parameters.AddWithValue("@nombre", direccion.Nombre);
                    comando.Parameters.AddWithValue("@detalleDireccion", direccion.DetalleDireccion);
                    comando.Parameters.AddWithValue("@ciudad", direccion.Ciudad);
                    comando.Parameters.AddWithValue("@region", direccion.Region);
                    comando.Parameters.AddWithValue("@codigoPostal", direccion.CodigoPostal);
                    comando.Parameters.AddWithValue("@pais", direccion.Pais);

                    return await comando.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        public async Task<bool> ActualizarAsync(Direccion direccion)
        {
            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();
                string query = "UPDATE Direcciones SET Nombre = @nombre, Direccion = @detalleDireccion, ciudad = @ciudad, region = @region, codigo_postal = @codigoPostal, pais = @pais WHERE IdDire = @idDire";
                
                using (var comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idDire", direccion.IdDire);
                    comando.Parameters.AddWithValue("@nombre", direccion.Nombre);
                    comando.Parameters.AddWithValue("@detalleDireccion", direccion.DetalleDireccion);
                    comando.Parameters.AddWithValue("@ciudad", direccion.Ciudad);
                    comando.Parameters.AddWithValue("@region", direccion.Region);
                    comando.Parameters.AddWithValue("@codigoPostal", direccion.CodigoPostal);
                    comando.Parameters.AddWithValue("@pais", direccion.Pais);

                    return await comando.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        public async Task<bool> EliminarAsync(int idDire)
        {
            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();
                string query = "DELETE FROM Direcciones WHERE IdDire = @idDire";
                
                using (var comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idDire", idDire);
                    return await comando.ExecuteNonQueryAsync() > 0;
                }
            }
        }
    }
}