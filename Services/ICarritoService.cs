using Legendary_Motorsport_Backend.Models;

namespace Legendary_Motorsport_Backend.Services
{
    public interface ICarritoService
    {
        Task<CarritoDto> ObtenerOCrearActivoAsync(int idCliente);
        Task<CarritoDto> AgregarOActualizarItemAsync(int idCliente, int idProducto, int cantidad);
        Task<CarritoDto> ActualizarCantidadAsync(int idCliente, int idProducto, int cantidad);
        Task<CarritoDto> EliminarItemAsync(int idCliente, int idProducto);
    }
}
