using Legendary_Motorsport_Backend.Models;

namespace Legendary_Motorsport_Backend.Repositories
{
    public interface ICarritoRepository
    {
        Task<CarritoDto> ObtenerOCrearActivoAsync(int idCliente);
        Task<CarritoDto?> ObtenerActivoAsync(int idCliente);
        Task<CarritoDto> AgregarOActualizarItemAsync(int idCliente, int idProducto, int cantidad);
        Task<CarritoDto> ActualizarCantidadAsync(int idCliente, int idProducto, int cantidad);
        Task<CarritoDto> EliminarItemAsync(int idCliente, int idProducto);
    }
}
