using Legendary_Motorsport_Backend.Models;
using Legendary_Motorsport_Backend.Repositories;

namespace Legendary_Motorsport_Backend.Services
{
    public class CarritoService : ICarritoService
    {
        private readonly ICarritoRepository _carritoRepository;

        public CarritoService(ICarritoRepository carritoRepository)
        {
            _carritoRepository = carritoRepository;
        }

        public async Task<CarritoDto> ObtenerOCrearActivoAsync(int idCliente)
        {
            return await _carritoRepository.ObtenerOCrearActivoAsync(idCliente);
        }

        public async Task<CarritoDto> AgregarOActualizarItemAsync(int idCliente, int idProducto, int cantidad)
        {
            return await _carritoRepository.AgregarOActualizarItemAsync(idCliente, idProducto, cantidad);
        }

        public async Task<CarritoDto> ActualizarCantidadAsync(int idCliente, int idProducto, int cantidad)
        {
            return await _carritoRepository.ActualizarCantidadAsync(idCliente, idProducto, cantidad);
        }

        public async Task<CarritoDto> EliminarItemAsync(int idCliente, int idProducto)
        {
            return await _carritoRepository.EliminarItemAsync(idCliente, idProducto);
        }
    }
}
