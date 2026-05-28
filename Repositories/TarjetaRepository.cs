using MySqlConnector;
using Legendary_Motorsport_Backend.Data;
using Legendary_Motorsport_Backend.Models;

namespace Legendary_Motorsport_Backend.Repositories
{
    public class TarjetaRepository : ITarjetaRepository
    {
        private readonly IConexionDb _conexionDb;

        public TarjetaRepository(IConexionDb conexionDb)
        {
            _conexionDb = conexionDb;
        }

        public async Task<IEnumerable<Tarjeta>> ObtenerPorClienteIdAsync(int idCliente)
        {
            var tarjetas = new List<Tarjeta>();
            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();
                string query = "SELECT IdTarjeta, Numero, Expiracion, CCV, Nombre, IdCliente FROM Tarjeta WHERE IdCliente = @idCliente";
                
                using (var comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idCliente", idCliente);
                    using (var lector = await comando.ExecuteReaderAsync())
                    {
                        while (await lector.ReadAsync())
                        {
                            tarjetas.Add(new Tarjeta
                            {
                                IdTarjeta = Convert.ToInt32(lector["IdTarjeta"]),
                                Numero = lector["Numero"].ToString(),
                                Expiracion = lector["Expiracion"].ToString(),
                                Ccv = lector["CCV"].ToString(),
                                Nombre = lector["Nombre"].ToString(),
                                IdCliente = Convert.ToInt32(lector["IdCliente"])
                            });
                        }
                    }
                }
            }
            return tarjetas;
        }

        public async Task<bool> CrearAsync(Tarjeta tarjeta)
        {
            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();
                string query = "INSERT INTO Tarjeta (Numero, Expiracion, CCV, Nombre, IdCliente) VALUES (@numero, @expiracion, @ccv, @nombre, @idCliente)";
                
                using (var comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@numero", tarjeta.Numero);
                    comando.Parameters.AddWithValue("@expiracion", tarjeta.Expiracion);
                    comando.Parameters.AddWithValue("@ccv", tarjeta.Ccv);
                    comando.Parameters.AddWithValue("@nombre", tarjeta.Nombre);
                    comando.Parameters.AddWithValue("@idCliente", tarjeta.IdCliente);

                    return await comando.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        public async Task<bool> ActualizarAsync(Tarjeta tarjeta)
        {
            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();
                string query = "UPDATE Tarjeta SET Numero = @numero, Expiracion = @expiracion, CCV = @ccv, Nombre = @nombre WHERE IdTarjeta = @idTarjeta";
                
                using (var comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idTarjeta", tarjeta.IdTarjeta);
                    comando.Parameters.AddWithValue("@numero", tarjeta.Numero);
                    comando.Parameters.AddWithValue("@expiracion", tarjeta.Expiracion);
                    comando.Parameters.AddWithValue("@ccv", tarjeta.Ccv);
                    comando.Parameters.AddWithValue("@nombre", tarjeta.Nombre);

                    return await comando.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        public async Task<bool> EliminarAsync(int idTarjeta)
        {
            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();
                string query = "DELETE FROM Tarjeta WHERE IdTarjeta = @idTarjeta";
                
                using (var comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idTarjeta", idTarjeta);
                    return await comando.ExecuteNonQueryAsync() > 0;
                }
            }
        }
    }
}