using MySqlConnector;
using Legendary_Motorsport_Backend.Data;
using Legendary_Motorsport_Backend.Models;

namespace Legendary_Motorsport_Backend.Repositories
{
    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly IConexionDb _conexionDb;

        public VehiculoRepository(IConexionDb conexionDb)
        {
            _conexionDb = conexionDb;
        }

        public async Task<IEnumerable<CatalogoResumenDto>> ObtenerCatalogoAsync()
        {
            var catalogo = new List<CatalogoResumenDto>();

            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();
                var query = @"
                    SELECT v.IdVehiculo,
                           v.marca,
                           v.modelo,
                           v.imagen_url,
                           COALESCE(MIN(i.Precio), 0) AS PrecioMinimo,
                           COALESCE(SUM(i.Stock), 0) AS StockTotal,
                           COUNT(DISTINCT i.Color) AS ColoresDisponibles
                    FROM Vehiculo v
                    LEFT JOIN Inventario i ON i.IdVehiculo = v.IdVehiculo
                    GROUP BY v.IdVehiculo, v.marca, v.modelo, v.imagen_url
                    ORDER BY v.marca, v.modelo;
                ";

                using (var comando = new MySqlCommand(query, conexion))
                using (var lector = await comando.ExecuteReaderAsync())
                {
                    while (await lector.ReadAsync())
                    {
                        catalogo.Add(new CatalogoResumenDto
                        {
                            IdVehiculo = Convert.ToInt32(lector["IdVehiculo"]),
                            Marca = lector["marca"].ToString(),
                            Modelo = lector["modelo"].ToString(),
                            ImagenUrl = lector["imagen_url"].ToString(),
                            PrecioMinimo = Convert.ToDecimal(lector["PrecioMinimo"]),
                            StockTotal = Convert.ToInt32(lector["StockTotal"]),
                            ColoresDisponibles = Convert.ToInt32(lector["ColoresDisponibles"])
                        });
                    }
                }
            }

            return catalogo;
        }

        public async Task<CatalogoDetalleDto?> ObtenerDetalleAsync(int idVehiculo)
        {
            CatalogoDetalleDto? detalle = null;

            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();

                var query = @"
                    SELECT v.IdVehiculo,
                           v.marca,
                           v.modelo,
                           v.imagen_url,
                           i.IdProducto,
                           i.Color,
                           i.Kilometraje,
                           i.Precio,
                           i.Stock
                    FROM Vehiculo v
                    LEFT JOIN Inventario i ON i.IdVehiculo = v.IdVehiculo
                    WHERE v.IdVehiculo = @idVehiculo
                    ORDER BY i.Precio ASC;
                ";

                using (var comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idVehiculo", idVehiculo);

                    using (var lector = await comando.ExecuteReaderAsync())
                    {
                        while (await lector.ReadAsync())
                        {
                            if (detalle == null)
                            {
                                detalle = new CatalogoDetalleDto
                                {
                                    IdVehiculo = Convert.ToInt32(lector["IdVehiculo"]),
                                    Marca = lector["marca"].ToString(),
                                    Modelo = lector["modelo"].ToString(),
                                    ImagenUrl = lector["imagen_url"].ToString()
                                };
                            }

                            if (lector["IdProducto"] != DBNull.Value)
                            {
                                detalle.Inventario.Add(new InventarioItemDto
                                {
                                    IdProducto = Convert.ToInt32(lector["IdProducto"]),
                                    Color = lector["Color"].ToString(),
                                    Kilometraje = Convert.ToInt32(lector["Kilometraje"]),
                                    Precio = Convert.ToDecimal(lector["Precio"]),
                                    Stock = Convert.ToInt32(lector["Stock"])
                                });
                            }
                        }
                    }
                }
            }

            return detalle;
        }
    }
}
