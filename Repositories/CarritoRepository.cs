using MySqlConnector;
using Legendary_Motorsport_Backend.Data;
using Legendary_Motorsport_Backend.Models;

namespace Legendary_Motorsport_Backend.Repositories
{
    public class CarritoRepository : ICarritoRepository
    {
        private readonly IConexionDb _conexionDb;

        public CarritoRepository(IConexionDb conexionDb)
        {
            _conexionDb = conexionDb;
        }

        public async Task<CarritoDto> ObtenerOCrearActivoAsync(int idCliente)
        {
            var carrito = await ObtenerActivoAsync(idCliente);
            if (carrito != null)
            {
                return carrito;
            }

            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();
                var query = "INSERT INTO Carrito (IdCliente, total, estado) VALUES (@idCliente, 0.00, 'Activo')";

                using (var comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idCliente", idCliente);
                    await comando.ExecuteNonQueryAsync();
                }
            }

            var creado = await ObtenerActivoAsync(idCliente);
            if (creado == null)
            {
                throw new InvalidOperationException("No se pudo crear el carrito activo.");
            }

            return creado;
        }

        public async Task<CarritoDto?> ObtenerActivoAsync(int idCliente)
        {
            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();
                var query = "SELECT IdCarrito, IdCliente, total, estado FROM Carrito WHERE IdCliente = @idCliente AND estado = 'Activo' LIMIT 1";

                using (var comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idCliente", idCliente);

                    using (var lector = await comando.ExecuteReaderAsync())
                    {
                        if (!await lector.ReadAsync())
                        {
                            return null;
                        }

                        var carrito = new CarritoDto
                        {
                            IdCarrito = Convert.ToInt32(lector["IdCarrito"]),
                            IdCliente = Convert.ToInt32(lector["IdCliente"]),
                            Total = Convert.ToDecimal(lector["total"]),
                            Estado = lector["estado"].ToString()
                        };

                        return carrito;
                    }
                }
            }
        }

        public async Task<CarritoDto> AgregarOActualizarItemAsync(int idCliente, int idProducto, int cantidad)
        {
            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad debe ser mayor a 0.");
            }

            var carrito = await ObtenerOCrearActivoAsync(idCliente);

            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();

                var inventarioQuery = "SELECT Stock, Precio FROM Inventario WHERE IdProducto = @idProducto";
                using (var inventarioCmd = new MySqlCommand(inventarioQuery, conexion))
                {
                    inventarioCmd.Parameters.AddWithValue("@idProducto", idProducto);
                    using (var lector = await inventarioCmd.ExecuteReaderAsync())
                    {
                        if (!await lector.ReadAsync())
                        {
                            throw new ArgumentException("Producto no encontrado.");
                        }

                        var stock = Convert.ToInt32(lector["Stock"]);
                        var precio = Convert.ToDecimal(lector["Precio"]);

                        if (stock <= 0)
                        {
                            throw new InvalidOperationException("No hay stock disponible.");
                        }

                        await lector.CloseAsync();

                        var existeQuery = "SELECT IdDetalleCarrito, Cantidad FROM DetalleCarrito WHERE IdCarrito = @idCarrito AND IdProducto = @idProducto";
                        using (var existeCmd = new MySqlCommand(existeQuery, conexion))
                        {
                            existeCmd.Parameters.AddWithValue("@idCarrito", carrito.IdCarrito);
                            existeCmd.Parameters.AddWithValue("@idProducto", idProducto);

                            using (var existeReader = await existeCmd.ExecuteReaderAsync())
                            {
                                if (await existeReader.ReadAsync())
                                {
                                    var idDetalle = Convert.ToInt32(existeReader["IdDetalleCarrito"]);
                                    var cantidadActual = Convert.ToInt32(existeReader["Cantidad"]);
                                    var nuevaCantidad = cantidadActual + cantidad;

                                    if (nuevaCantidad > stock)
                                    {
                                        throw new InvalidOperationException("Stock insuficiente para la cantidad solicitada.");
                                    }

                                    await existeReader.CloseAsync();

                                    var updateQuery = "UPDATE DetalleCarrito SET Cantidad = @cantidad, Precio = @precio WHERE IdDetalleCarrito = @idDetalle";
                                    using (var updateCmd = new MySqlCommand(updateQuery, conexion))
                                    {
                                        updateCmd.Parameters.AddWithValue("@cantidad", nuevaCantidad);
                                        updateCmd.Parameters.AddWithValue("@precio", precio);
                                        updateCmd.Parameters.AddWithValue("@idDetalle", idDetalle);
                                        await updateCmd.ExecuteNonQueryAsync();
                                    }
                                }
                                else
                                {
                                    await existeReader.CloseAsync();

                                    if (cantidad > stock)
                                    {
                                        throw new InvalidOperationException("Stock insuficiente para la cantidad solicitada.");
                                    }

                                    var insertQuery = "INSERT INTO DetalleCarrito (IdCarrito, IdProducto, Cantidad, Precio) VALUES (@idCarrito, @idProducto, @cantidad, @precio)";
                                    using (var insertCmd = new MySqlCommand(insertQuery, conexion))
                                    {
                                        insertCmd.Parameters.AddWithValue("@idCarrito", carrito.IdCarrito);
                                        insertCmd.Parameters.AddWithValue("@idProducto", idProducto);
                                        insertCmd.Parameters.AddWithValue("@cantidad", cantidad);
                                        insertCmd.Parameters.AddWithValue("@precio", precio);
                                        await insertCmd.ExecuteNonQueryAsync();
                                    }
                                }
                            }
                        }
                    }
                }

                await RecalcularTotalAsync(conexion, carrito.IdCarrito);
            }

            return await ObtenerCarritoDetalleAsync(idCliente, carrito.IdCarrito);
        }

        public async Task<CarritoDto> ActualizarCantidadAsync(int idCliente, int idProducto, int cantidad)
        {
            if (cantidad <= 0)
            {
                return await EliminarItemAsync(idCliente, idProducto);
            }

            var carrito = await ObtenerOCrearActivoAsync(idCliente);

            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();

                var inventarioQuery = "SELECT Stock, Precio FROM Inventario WHERE IdProducto = @idProducto";
                using (var inventarioCmd = new MySqlCommand(inventarioQuery, conexion))
                {
                    inventarioCmd.Parameters.AddWithValue("@idProducto", idProducto);
                    using (var lector = await inventarioCmd.ExecuteReaderAsync())
                    {
                        if (!await lector.ReadAsync())
                        {
                            throw new ArgumentException("Producto no encontrado.");
                        }

                        var stock = Convert.ToInt32(lector["Stock"]);
                        var precio = Convert.ToDecimal(lector["Precio"]);

                        if (stock <= 0)
                        {
                            throw new InvalidOperationException("No hay stock disponible.");
                        }

                        await lector.CloseAsync();

                        if (cantidad > stock)
                        {
                            throw new InvalidOperationException("Stock insuficiente para la cantidad solicitada.");
                        }

                        var updateQuery = "UPDATE DetalleCarrito SET Cantidad = @cantidad, Precio = @precio WHERE IdCarrito = @idCarrito AND IdProducto = @idProducto";
                        using (var updateCmd = new MySqlCommand(updateQuery, conexion))
                        {
                            updateCmd.Parameters.AddWithValue("@cantidad", cantidad);
                            updateCmd.Parameters.AddWithValue("@precio", precio);
                            updateCmd.Parameters.AddWithValue("@idCarrito", carrito.IdCarrito);
                            updateCmd.Parameters.AddWithValue("@idProducto", idProducto);
                            await updateCmd.ExecuteNonQueryAsync();
                        }
                    }
                }

                await RecalcularTotalAsync(conexion, carrito.IdCarrito);
            }

            return await ObtenerCarritoDetalleAsync(idCliente, carrito.IdCarrito);
        }

        public async Task<CarritoDto> EliminarItemAsync(int idCliente, int idProducto)
        {
            var carrito = await ObtenerOCrearActivoAsync(idCliente);

            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();
                var query = "DELETE FROM DetalleCarrito WHERE IdCarrito = @idCarrito AND IdProducto = @idProducto";

                using (var comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idCarrito", carrito.IdCarrito);
                    comando.Parameters.AddWithValue("@idProducto", idProducto);
                    await comando.ExecuteNonQueryAsync();
                }

                await RecalcularTotalAsync(conexion, carrito.IdCarrito);
            }

            return await ObtenerCarritoDetalleAsync(idCliente, carrito.IdCarrito);
        }

        private async Task RecalcularTotalAsync(MySqlConnection conexion, int idCarrito)
        {
            var query = "UPDATE Carrito SET total = (SELECT IFNULL(SUM(Cantidad * Precio), 0) FROM DetalleCarrito WHERE IdCarrito = @idCarrito) WHERE IdCarrito = @idCarrito";
            using (var comando = new MySqlCommand(query, conexion))
            {
                comando.Parameters.AddWithValue("@idCarrito", idCarrito);
                await comando.ExecuteNonQueryAsync();
            }
        }

        private async Task<CarritoDto> ObtenerCarritoDetalleAsync(int idCliente, int idCarrito)
        {
            var carrito = new CarritoDto
            {
                IdCarrito = idCarrito,
                IdCliente = idCliente,
                Estado = "Activo"
            };

            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();

                var headerQuery = "SELECT total, estado FROM Carrito WHERE IdCarrito = @idCarrito";
                using (var headerCmd = new MySqlCommand(headerQuery, conexion))
                {
                    headerCmd.Parameters.AddWithValue("@idCarrito", idCarrito);
                    using (var headerReader = await headerCmd.ExecuteReaderAsync())
                    {
                        if (await headerReader.ReadAsync())
                        {
                            carrito.Total = Convert.ToDecimal(headerReader["total"]);
                            carrito.Estado = headerReader["estado"].ToString();
                        }
                    }
                }

                var itemsQuery = @"
                    SELECT dc.IdDetalleCarrito,
                           dc.IdProducto,
                           dc.Cantidad,
                           dc.Precio,
                           (dc.Cantidad * dc.Precio) AS Subtotal,
                           v.marca,
                           v.modelo,
                           v.imagen_url,
                           i.Color,
                           i.Kilometraje
                    FROM DetalleCarrito dc
                    INNER JOIN Inventario i ON i.IdProducto = dc.IdProducto
                    INNER JOIN Vehiculo v ON v.IdVehiculo = i.IdVehiculo
                    WHERE dc.IdCarrito = @idCarrito
                    ORDER BY dc.IdDetalleCarrito DESC;
                ";

                using (var comando = new MySqlCommand(itemsQuery, conexion))
                {
                    comando.Parameters.AddWithValue("@idCarrito", idCarrito);
                    using (var lector = await comando.ExecuteReaderAsync())
                    {
                        while (await lector.ReadAsync())
                        {
                            carrito.Items.Add(new CarritoItemDto
                            {
                                IdDetalleCarrito = Convert.ToInt32(lector["IdDetalleCarrito"]),
                                IdProducto = Convert.ToInt32(lector["IdProducto"]),
                                Cantidad = Convert.ToInt32(lector["Cantidad"]),
                                Precio = Convert.ToDecimal(lector["Precio"]),
                                Subtotal = Convert.ToDecimal(lector["Subtotal"]),
                                Marca = lector["marca"].ToString(),
                                Modelo = lector["modelo"].ToString(),
                                Color = lector["Color"].ToString(),
                                Kilometraje = Convert.ToInt32(lector["Kilometraje"]),
                                ImagenUrl = lector["imagen_url"].ToString()
                            });
                        }
                    }
                }
            }

            return carrito;
        }
    }
}
