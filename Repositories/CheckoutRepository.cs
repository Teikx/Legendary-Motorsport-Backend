using MySqlConnector;
using Legendary_Motorsport_Backend.Data;
using Legendary_Motorsport_Backend.Models;

namespace Legendary_Motorsport_Backend.Repositories
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly IConexionDb _conexionDb;

        public CheckoutRepository(IConexionDb conexionDb)
        {
            _conexionDb = conexionDb;
        }

        public async Task<CheckoutResponse> ProcesarCheckoutAsync(int idCliente, int idTarjeta, int idDireccion)
        {
            using (var conexion = _conexionDb.ObtenerConexion())
            {
                await conexion.OpenAsync();
                using (var transaccion = await conexion.BeginTransactionAsync())
                {
                    try
                    {
                        var idCarrito = await ObtenerCarritoActivoAsync(conexion, transaccion, idCliente);
                        var items = await ObtenerItemsCarritoAsync(conexion, transaccion, idCarrito);

                        if (items.Count == 0)
                        {
                            throw new InvalidOperationException("El carrito no tiene items.");
                        }

                        var response = new CheckoutResponse();
                        decimal total = 0m;

                        foreach (var item in items)
                        {
                            var inventario = await ObtenerInventarioAsync(conexion, transaccion, item.IdProducto);
                            if (inventario.Stock < item.Cantidad)
                            {
                                throw new InvalidOperationException("Stock insuficiente para el producto solicitado.");
                            }

                            var subtotal = item.Cantidad * inventario.Precio;
                            total += subtotal;

                            response.Items.Add(new CheckoutItemDto
                            {
                                IdProducto = item.IdProducto,
                                Cantidad = item.Cantidad,
                                Precio = inventario.Precio,
                                Subtotal = subtotal
                            });
                        }

                        var idCompra = await InsertarCompraAsync(conexion, transaccion, idCliente, idTarjeta, idDireccion, total);
                        response.IdCompra = idCompra;
                        response.Total = total;

                        foreach (var item in response.Items)
                        {
                            await InsertarDetalleCompraAsync(conexion, transaccion, idCompra, item);
                            await DescontarStockAsync(conexion, transaccion, item.IdProducto, item.Cantidad);
                        }

                        await CerrarCarritoAsync(conexion, transaccion, idCarrito, total);
                        await transaccion.CommitAsync();

                        return response;
                    }
                    catch
                    {
                        await transaccion.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        private async Task<int> ObtenerCarritoActivoAsync(MySqlConnection conexion, MySqlTransaction transaccion, int idCliente)
        {
            var query = "SELECT IdCarrito FROM Carrito WHERE IdCliente = @idCliente AND estado = 'Activo' LIMIT 1 FOR UPDATE";
            using (var comando = new MySqlCommand(query, conexion, transaccion))
            {
                comando.Parameters.AddWithValue("@idCliente", idCliente);
                var result = await comando.ExecuteScalarAsync();
                if (result == null)
                {
                    throw new InvalidOperationException("No hay carrito activo para este cliente.");
                }

                return Convert.ToInt32(result);
            }
        }

        private async Task<List<(int IdProducto, int Cantidad)>> ObtenerItemsCarritoAsync(MySqlConnection conexion, MySqlTransaction transaccion, int idCarrito)
        {
            var items = new List<(int IdProducto, int Cantidad)>();
            var query = "SELECT IdProducto, Cantidad FROM DetalleCarrito WHERE IdCarrito = @idCarrito";

            using (var comando = new MySqlCommand(query, conexion, transaccion))
            {
                comando.Parameters.AddWithValue("@idCarrito", idCarrito);
                using (var lector = await comando.ExecuteReaderAsync())
                {
                    while (await lector.ReadAsync())
                    {
                        items.Add((
                            Convert.ToInt32(lector["IdProducto"]),
                            Convert.ToInt32(lector["Cantidad"])));
                    }
                }
            }

            return items;
        }

        private async Task<(int Stock, decimal Precio)> ObtenerInventarioAsync(MySqlConnection conexion, MySqlTransaction transaccion, int idProducto)
        {
            var query = "SELECT Stock, Precio FROM Inventario WHERE IdProducto = @idProducto FOR UPDATE";
            using (var comando = new MySqlCommand(query, conexion, transaccion))
            {
                comando.Parameters.AddWithValue("@idProducto", idProducto);
                using (var lector = await comando.ExecuteReaderAsync())
                {
                    if (!await lector.ReadAsync())
                    {
                        throw new InvalidOperationException("Producto no encontrado en inventario.");
                    }

                    return (
                        Convert.ToInt32(lector["Stock"]),
                        Convert.ToDecimal(lector["Precio"])) ;
                }
            }
        }

        private async Task<int> InsertarCompraAsync(MySqlConnection conexion, MySqlTransaction transaccion, int idCliente, int idTarjeta, int idDireccion, decimal total)
        {
            var query = "INSERT INTO Compra (IdCliente, IdTarjeta, IdDireccion, Total) VALUES (@idCliente, @idTarjeta, @idDireccion, @total); SELECT LAST_INSERT_ID();";
            using (var comando = new MySqlCommand(query, conexion, transaccion))
            {
                comando.Parameters.AddWithValue("@idCliente", idCliente);
                comando.Parameters.AddWithValue("@idTarjeta", idTarjeta);
                comando.Parameters.AddWithValue("@idDireccion", idDireccion);
                comando.Parameters.AddWithValue("@total", total);

                var result = await comando.ExecuteScalarAsync();
                return Convert.ToInt32(result);
            }
        }

        private async Task InsertarDetalleCompraAsync(MySqlConnection conexion, MySqlTransaction transaccion, int idCompra, CheckoutItemDto item)
        {
            var query = "INSERT INTO DetalleCompra (IdCompra, IdProducto, Cantidad, Precio) VALUES (@idCompra, @idProducto, @cantidad, @precio)";
            using (var comando = new MySqlCommand(query, conexion, transaccion))
            {
                comando.Parameters.AddWithValue("@idCompra", idCompra);
                comando.Parameters.AddWithValue("@idProducto", item.IdProducto);
                comando.Parameters.AddWithValue("@cantidad", item.Cantidad);
                comando.Parameters.AddWithValue("@precio", item.Precio);
                await comando.ExecuteNonQueryAsync();
            }
        }

        private async Task DescontarStockAsync(MySqlConnection conexion, MySqlTransaction transaccion, int idProducto, int cantidad)
        {
            var query = "UPDATE Inventario SET Stock = Stock - @cantidad WHERE IdProducto = @idProducto AND Stock >= @cantidad";
            using (var comando = new MySqlCommand(query, conexion, transaccion))
            {
                comando.Parameters.AddWithValue("@cantidad", cantidad);
                comando.Parameters.AddWithValue("@idProducto", idProducto);
                var rows = await comando.ExecuteNonQueryAsync();
                if (rows == 0)
                {
                    throw new InvalidOperationException("No se pudo actualizar el stock.");
                }
            }
        }

        private async Task CerrarCarritoAsync(MySqlConnection conexion, MySqlTransaction transaccion, int idCarrito, decimal total)
        {
            var query = "UPDATE Carrito SET estado = 'Comprado', total = @total WHERE IdCarrito = @idCarrito";
            using (var comando = new MySqlCommand(query, conexion, transaccion))
            {
                comando.Parameters.AddWithValue("@idCarrito", idCarrito);
                comando.Parameters.AddWithValue("@total", total);
                await comando.ExecuteNonQueryAsync();
            }
        }
    }
}
